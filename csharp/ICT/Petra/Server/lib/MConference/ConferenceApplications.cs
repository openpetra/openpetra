//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// Manage Conference applications
    /// </summary>
    public class TApplicationManagement
    {
        /// <summary>
        /// return a list of all applicants for a given event
        /// </summary>
        /// <param name="AEventCode"></param>
        /// <param name="ARegisteringOffice"></param>
        /// <returns></returns>
        public static ConferenceApplicationTDS GetApplications(string AEventCode, Int64 ARegisteringOffice)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            PmShortTermApplicationRow TemplateRow = MainDS.PmShortTermApplication.NewRowTyped(false);

            TemplateRow.RegistrationOffice = ARegisteringOffice;
            TemplateRow.ConfirmedOptionCode = AEventCode;
            PmShortTermApplicationAccess.LoadUsingTemplate(MainDS, TemplateRow, Transaction);

            foreach (PmShortTermApplicationRow shortTermRow in MainDS.PmShortTermApplication.Rows)
            {
                PPersonAccess.LoadByPrimaryKey(MainDS, shortTermRow.PartnerKey, Transaction);
                PmGeneralApplicationAccess.LoadByPrimaryKey(MainDS, shortTermRow.PartnerKey,
                    shortTermRow.ApplicationKey,
                    shortTermRow.RegistrationOffice,
                    Transaction);

                MainDS.PPerson.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        PPersonTable.GetPartnerKeyDBName(),
                        shortTermRow.PartnerKey);
                MainDS.PmGeneralApplication.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        PmGeneralApplicationTable.GetPartnerKeyDBName(),
                        shortTermRow.PartnerKey);

                PPersonRow Person = (PPersonRow)MainDS.PPerson.DefaultView[0].Row;
                PmGeneralApplicationRow GeneralApplication = (PmGeneralApplicationRow)MainDS.PmGeneralApplication.DefaultView[0].Row;

                ConferenceApplicationTDSApplicationGridRow newRow = MainDS.ApplicationGrid.NewRowTyped();
                newRow.PartnerKey = shortTermRow.PartnerKey;
                newRow.FirstName = Person.FirstName;
                newRow.FamilyName = Person.FamilyName;

                if (!Person.IsDateOfBirthNull())
                {
                    newRow.DateOfBirth = Person.DateOfBirth;
                }

                newRow.Gender = Person.Gender;
                newRow.GenAppDate = GeneralApplication.GenAppDate;

                // TODO: display the description of that application status
                newRow.GenApplicationStatus = GeneralApplication.GenApplicationStatus;
                newRow.StCongressCode = shortTermRow.StCongressCode;
                MainDS.ApplicationGrid.Rows.Add(newRow);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            MainDS.AcceptChanges();

            return MainDS;
        }

        /// <summary>
        /// store the adjusted applications to the database
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        public static TSubmitChangesResult SaveApplications(ref ConferenceApplicationTDS AMainDS)
        {
            try
            {
                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        AMainDS.PPerson.DefaultView.RowFilter =
                            String.Format("{0}={1}",
                                PPersonTable.GetPartnerKeyDBName(),
                                row.PartnerKey);
                        AMainDS.PmShortTermApplication.DefaultView.RowFilter =
                            String.Format("{0}={1}",
                                PmShortTermApplicationTable.GetPartnerKeyDBName(),
                                row.PartnerKey);
                        AMainDS.PmGeneralApplication.DefaultView.RowFilter =
                            String.Format("{0}={1}",
                                PmGeneralApplicationTable.GetPartnerKeyDBName(),
                                row.PartnerKey);

                        PPersonRow Person = (PPersonRow)AMainDS.PPerson.DefaultView[0].Row;
                        PmShortTermApplicationRow ShortTermApplication = (PmShortTermApplicationRow)AMainDS.PmShortTermApplication.DefaultView[0].Row;
                        PmGeneralApplicationRow GeneralApplication = (PmGeneralApplicationRow)AMainDS.PmGeneralApplication.DefaultView[0].Row;

                        Person.FirstName = row.FirstName;
                        Person.FamilyName = row.FamilyName;

                        if (row.DateOfBirth.HasValue)
                        {
                            Person.DateOfBirth = row.DateOfBirth;
                        }
                        else
                        {
                            Person.SetDateOfBirthNull();
                        }

                        Person.Gender = row.Gender;
                        GeneralApplication.GenApplicationStatus = row.GenApplicationStatus;
                        ShortTermApplication.StCongressCode = row.StCongressCode;
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                return TSubmitChangesResult.scrError;
            }

            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult result = ConferenceApplicationTDSAccess.SubmitChanges(AMainDS, out VerificationResult);

            AMainDS.AcceptChanges();

            return result;
        }
    }
}