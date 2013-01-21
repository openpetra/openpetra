//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.IO;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// print finance reports for the registration offices
    /// </summary>
    public class TConferenceFinanceReports
    {
        /// <summary>
        /// print finance report for the registration office
        /// </summary>
        public static string PrintFinanceReport(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ASelectedRegistrationOffice)
        {
            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

            DateTime LatestFreeCancelledDate = DateTime.ParseExact(TAppSettingsManager.GetValue(
                    "ConferenceTool.LatestFreeCancelledDate"), "yyyy/MM/dd", null);
            DateTime LatestFreeCancelledDate50 = DateTime.ParseExact(TAppSettingsManager.GetValue(
                    "ConferenceTool.LatestFreeCancelledDate50"), "yyyy/MM/dd", null);

            try
            {
                ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

                TApplicationManagement.GetApplications(
                    ref MainDS,
                    AEventPartnerKey,
                    AEventCode,
                    "all",
                    ASelectedRegistrationOffice,
                    null,
                    false);

                MainDS.PmShortTermApplication.DefaultView.Sort =
                    PmShortTermApplicationTable.GetPartnerKeyDBName();

                MainDS.ApplicationGrid.DefaultView.Sort =
                    ConferenceApplicationTDSApplicationGridTable.GetStCongressCodeDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

                SortedList <string, List <string>>roles = new SortedList <string, List <string>>();

                // go through all applicants
                foreach (DataRowView rv in MainDS.ApplicationGrid.DefaultView)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant = (ConferenceApplicationTDSApplicationGridRow)rv.Row;

                    int IndexGeneralApp = MainDS.PmGeneralApplication.DefaultView.Find(
                        new object[] { applicant.PartnerKey, applicant.ApplicationKey, applicant.RegistrationOffice });
                    PmGeneralApplicationRow appRow = (PmGeneralApplicationRow)MainDS.PmGeneralApplication.DefaultView[IndexGeneralApp].Row;

                    int IndexShorttermApp = MainDS.PmShortTermApplication.DefaultView.Find(applicant.PartnerKey);
                    PmShortTermApplicationRow shorttermRow =
                        (PmShortTermApplicationRow)MainDS.PmShortTermApplication.DefaultView[IndexShorttermApp].Row;

                    Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(applicant.JSONData);

                    DateTime? DateCancelled = new Nullable <DateTime>();

                    if (shorttermRow.StFieldCharged != ASelectedRegistrationOffice)
                    {
                        // only consider applications that are charged to this office
                        continue;
                    }

                    if (appRow.IsGenAppSendFldAcceptDateNull())
                    {
                        // ignore partners that have never been accepted.
                        continue;
                    }

                    string pay50Percent = string.Empty;

                    if (applicant.GenApplicationStatus.StartsWith("C") || applicant.GenApplicationStatus.StartsWith("R"))
                    {
                        DateCancelled = appRow.GenAppCancelled;

                        if (!appRow.GenAppCancelled.HasValue)
                        {
                            TLogging.Log("no GenAppCancelled Value " + applicant.PartnerKey.ToString());
                        }
                        else if (appRow.GenAppCancelled.Value <= LatestFreeCancelledDate)
                        {
                            continue;
                        }
                        else if (appRow.GenAppCancelled.Value <= LatestFreeCancelledDate50)
                        {
                            pay50Percent = "X";
                        }
                    }
                    else if (applicant.GenApplicationStatus != "A")
                    {
                        continue;
                    }

                    if (!roles.ContainsKey(applicant.StCongressCode))
                    {
                        roles.Add(applicant.StCongressCode, new List <string>());
                    }

                    string participantValues = string.Empty;

                    if (!applicant.IsPersonKeyNull())
                    {
                        participantValues = StringHelper.AddCSV(participantValues, applicant.PersonKey.ToString());
                    }
                    else
                    {
                        participantValues = StringHelper.AddCSV(participantValues, applicant.PartnerKey.ToString());
                    }

                    Catalog.SetCulture(CultureInfo.InvariantCulture);

                    participantValues = StringHelper.AddCSV(participantValues, applicant.FamilyName);
                    participantValues = StringHelper.AddCSV(participantValues, applicant.FirstName);
                    participantValues = StringHelper.AddCSV(participantValues, applicant.Gender);

                    if (applicant.DateOfBirth.HasValue)
                    {
                        participantValues = StringHelper.AddCSV(participantValues, applicant.DateOfBirth.Value.ToString("dd-MMM-yyyy"));
                    }
                    else
                    {
                        participantValues = StringHelper.AddCSV(participantValues, "N/A");
                    }

                    DateTime ArrivalDate = DateTime.ParseExact(TAppSettingsManager.GetValue(
                            "ConferenceTool.DefaultArrivalDate"), "yyyy/MM/dd", null);

                    if (shorttermRow.Arrival.HasValue)
                    {
                        ArrivalDate = shorttermRow.Arrival.Value;
                    }

                    if (DateCancelled.HasValue)
                    {
                        participantValues = StringHelper.AddCSV(participantValues, DateCancelled.Value.ToString("dd-MMM-yyyy") + " C");
                    }
                    else
                    {
                        participantValues = StringHelper.AddCSV(participantValues, ArrivalDate.ToString("dd-MMM-yyyy"));
                    }

                    if (applicant.DateOfBirth.HasValue)
                    {
                        participantValues = StringHelper.AddCSV(participantValues,
                            (TApplicationManagement.CalculateAge(applicant.DateOfBirth, ArrivalDate) >=
                             TAppSettingsManager.GetInt32("ConferenceTool.OldieIncreasedTaxes")) ? "X" : string.Empty);
                    }
                    else
                    {
                        participantValues = StringHelper.AddCSV(participantValues, "X");
                    }

                    participantValues = StringHelper.AddCSV(participantValues,
                        (rawDataObject.Contains(
                             "SecondSibling") && rawDataObject["SecondSibling"].ToString().ToLower() == "true") ? "X" : string.Empty);
                    participantValues = StringHelper.AddCSV(participantValues,
                        (rawDataObject.Contains(
                             "CancelledByFinanceOffice") && rawDataObject["CancelledByFinanceOffice"].ToString().ToLower() ==
                         "true") ? "X" : string.Empty);
                    participantValues = StringHelper.AddCSV(participantValues, pay50Percent);

                    roles[applicant.StCongressCode].Add(participantValues);
                }

                string TemplateFilename = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                    "ConferenceInvoice",
                    "",
                    "",
                    "html");

                PPartnerTable regOffices = TApplicationManagement.GetRegistrationOffices();
                string RegistrationOfficeName = ASelectedRegistrationOffice.ToString();

                try
                {
                    RegistrationOfficeName =
                        ((PPartnerRow)regOffices.DefaultView[regOffices.DefaultView.Find(ASelectedRegistrationOffice)].Row).PartnerShortName;
                }
                catch (Exception)
                {
                }

                string ResultDocument = TFormLettersTools.PrintReport(TemplateFilename, roles, RegistrationOfficeName, false);

                if (ResultDocument.Length == 0)
                {
                    return String.Empty;
                }

                string PDFPath = TFormLettersTools.GeneratePDFFromHTML(ResultDocument,
                    TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                    "reports");

                return PDFPath;
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());
                throw;
            }
            finally
            {
                Catalog.SetCulture(OrigCulture);
            }
        }
    }
}