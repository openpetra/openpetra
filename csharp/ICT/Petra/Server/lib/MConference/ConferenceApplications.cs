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
        public static ConferenceApplicationTDSApplicationGridTable GetApplications(string AEventCode, Int64 ARegisteringOffice)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            PmShortTermApplicationRow TemplateRow = MainDS.PmShortTermApplication.NewRowTyped(false);

            TemplateRow.RegistrationOffice = ARegisteringOffice;
            TemplateRow.ConfirmedOptionCode = AEventCode;
            PmShortTermApplicationAccess.LoadUsingTemplate(MainDS, TemplateRow, Transaction);

            foreach (PmShortTermApplicationRow shortTermRow in MainDS.PmShortTermApplication.Rows)
            {
                PPersonTable personTable = PPersonAccess.LoadByPrimaryKey(shortTermRow.PartnerKey, Transaction);
                PmGeneralApplicationTable genAppTable = PmGeneralApplicationAccess.LoadByPrimaryKey(shortTermRow.PartnerKey,
                    shortTermRow.ApplicationKey,
                    shortTermRow.RegistrationOffice,
                    Transaction);

                ConferenceApplicationTDSApplicationGridRow newRow = MainDS.ApplicationGrid.NewRowTyped();
                newRow.PartnerKey = shortTermRow.PartnerKey;
                newRow.FirstName = personTable[0].FirstName;
                newRow.FamilyName = personTable[0].FamilyName;

                if (!personTable[0].IsDateOfBirthNull())
                {
                    newRow.DateOfBirth = personTable[0].DateOfBirth;
                }

                newRow.Gender = personTable[0].Gender;
                newRow.GenAppDate = genAppTable[0].GenAppDate;

                // TODO: display the description of that application status
                newRow.GenApplicationStatus = genAppTable[0].GenApplicationStatus;
                newRow.StCongressCode = shortTermRow.StCongressCode;
                MainDS.ApplicationGrid.Rows.Add(newRow);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            return MainDS.ApplicationGrid;
        }
    }
}