//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Net.Mail;
using System.Collections.Generic;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Import;
using Ict.Petra.Server.MPartner.ImportExport;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// For creating gift batches for conference payments
    /// </summary>
    public class TImportFellowshipGroups
    {
        private static PmShortTermApplicationRow FindShortTermApplication(ConferenceApplicationTDS AMainDS,
            ref Int64 APartnerKey,
            string LastName,
            string FirstName)
        {
            if ((APartnerKey <= 0) && (LastName.Length > 0) && (FirstName.Length > 0))
            {
                // try to find the partner key from the name
                AMainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                                                           ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();
                Int32 index = AMainDS.ApplicationGrid.DefaultView.Find(new object[] { LastName, FirstName });

                if (index == -1)
                {
                    TLogging.Log("Import Fellowship groups: Cannot find attendee " + FirstName + " " + LastName);
                    return null;
                }

                ConferenceApplicationTDSApplicationGridRow ApplicantRow =
                    (ConferenceApplicationTDSApplicationGridRow)AMainDS.ApplicationGrid.DefaultView[index].Row;
                APartnerKey = ApplicantRow.PartnerKey;
            }
            else
            {
                // is this the person key from the local Petra database, or the registration key?
                AMainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetPersonKeyDBName();
                Int32 index = AMainDS.ApplicationGrid.DefaultView.Find(APartnerKey);

                if (index != -1)
                {
                    ConferenceApplicationTDSApplicationGridRow ApplicantRow =
                        (ConferenceApplicationTDSApplicationGridRow)AMainDS.ApplicationGrid.DefaultView[index].Row;
                    APartnerKey = ApplicantRow.PartnerKey;
                }
            }

            Int32 indexShorttermApp = AMainDS.PmShortTermApplication.DefaultView.Find(APartnerKey);

            if (indexShorttermApp == -1)
            {
                return null;
            }

            return (PmShortTermApplicationRow)AMainDS.PmShortTermApplication.DefaultView[indexShorttermApp].Row;
        }

        /// <summary>
        /// import fellowship groups.
        /// Column1 is Lastname, Column2 is Firstname, Column3 is PartnerKey, Column4 is GroupCode
        /// </summary>
        static public bool ImportFellowshipGroups(
            string AFellowshipGroupsCSV,
            Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ARegisteringOffice)
        {
            string InputSeparator = ",";

            if (AFellowshipGroupsCSV.Contains("\t"))
            {
                InputSeparator = "\t";
            }
            else if (AFellowshipGroupsCSV.Contains(";"))
            {
                InputSeparator = ";";
            }

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();
            TApplicationManagement.GetApplications(
                ref MainDS,
                AEventPartnerKey,
                AEventCode,
                "accepted",
                ARegisteringOffice,
                String.Empty,
                true);

            try
            {
                MainDS.PmShortTermApplication.DefaultView.Sort = PmShortTermApplicationTable.GetPartnerKeyDBName();

                string[] InputLines = AFellowshipGroupsCSV.Replace("\r", "").Split(new char[] { '\n' });

                int RowCount = 0;

                foreach (string InputLine in InputLines)
                {
                    RowCount++;

                    string line = InputLine;

                    string LastName = StringHelper.GetNextCSV(ref line, InputSeparator, "");
                    string FirstName = StringHelper.GetNextCSV(ref line, InputSeparator, "");
                    Int64 PartnerKey = StringHelper.TryStrToInt(StringHelper.GetNextCSV(ref line, InputSeparator, ""), -1);
                    string GroupCode = StringHelper.GetNextCSV(ref line, InputSeparator, "");

                    PmShortTermApplicationRow ShorttermAppRow = FindShortTermApplication(MainDS, ref PartnerKey, String.Empty, string.Empty);

                    if (ShorttermAppRow == null)
                    {
                        PartnerKey = -1;
                        ShorttermAppRow = FindShortTermApplication(MainDS, ref PartnerKey, LastName, FirstName);
                    }

                    if (ShorttermAppRow == null)
                    {
                        TLogging.Log(
                            "Import Fellowship groups: Cannot find shortterm application for attendee " + FirstName + " " + LastName + " " +
                            PartnerKey.ToString());
                        continue;
                    }

                    if (ShorttermAppRow.StFgCode.Length == 0)
                    {
                        ShorttermAppRow.StFgCode = GroupCode;
                    }
                    else if (ShorttermAppRow.StFgCode != GroupCode)
                    {
                        TLogging.Log("we do not overwrite fellowship groups: " + FirstName + " " + LastName + " " + PartnerKey.ToString());
                    }
                }

                TVerificationResultCollection VerificationResult;

                ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);
            }
            catch (Exception ex)
            {
                TLogging.Log("Importing Fellowship groups: " + ex.Message);
                return false;
            }

            return true;
        }
    }
}