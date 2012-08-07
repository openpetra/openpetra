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
using System.Xml;
using System.Globalization;
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
    /// for the Boundaries team, print a report for each home office rep for each day with the list of rebukes
    /// </summary>
    public class TRebukeReport
    {
        /// <summary>
        /// print a report for each home office rep for each day with the list of rebukes
        /// </summary>
        public static string PrintRebukeReport(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ASelectedRegistrationOffice,
            DateTime AAllRebukesOnThisDay)
        {
            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

            try
            {
                ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

                TApplicationManagement.GetApplications(
                    ref MainDS,
                    AEventPartnerKey,
                    AEventCode,
                    "accepted",
                    ASelectedRegistrationOffice,
                    null,
                    false);

                PPartnerTable regOffices = TApplicationManagement.GetRegistrationOffices();

                MainDS.ApplicationGrid.DefaultView.Sort =
                    ConferenceApplicationTDSApplicationGridTable.GetRegistrationOfficeDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

                // get index of boundaries datalabel
                Int32 RebukeLabelID;
                try
                {
                    Int32 IndexLabelRebuke = MainDS.PDataLabel.DefaultView.Find("Rebukes");
                    RebukeLabelID = ((PDataLabelRow)MainDS.PDataLabel.DefaultView[IndexLabelRebuke].Row).Key;
                }
                catch (Exception)
                {
                    throw new Exception("missing rebukes data label");
                }

                SortedList <string, List <string>>rebukes = new SortedList <string, List <string>>();

                foreach (DataRowView partnerrowview in regOffices.DefaultView)
                {
                    PPartnerRow partnerrow = (PPartnerRow)partnerrowview.Row;
                    rebukes.Add(partnerrow.PartnerShortName, new List <string>());
                }

                // go through all applicants
                foreach (DataRowView rv in MainDS.ApplicationGrid.DefaultView)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant = (ConferenceApplicationTDSApplicationGridRow)rv.Row;

                    int IndexLabel = MainDS.PDataLabelValuePartner.DefaultView.Find(new object[] { RebukeLabelID, applicant.PartnerKey });

                    if (IndexLabel == -1)
                    {
                        continue;
                    }

                    string RebukeNotes = ((PDataLabelValuePartnerRow)MainDS.PDataLabelValuePartner.DefaultView[IndexLabel].Row).ValueChar;

                    object obj = Jayrock.Json.Conversion.JsonConvert.Import(RebukeNotes);

                    if (!(obj is Jayrock.Json.JsonArray))
                    {
                        // somehow the RebukesNotes string just contains two double quotes, but not a list
                        continue;
                    }

                    Jayrock.Json.JsonArray list = (Jayrock.Json.JsonArray)obj;

                    foreach (Jayrock.Json.JsonObject element in list)
                    {
                        if (element.Contains("When")
                            && (Convert.ToDateTime(element["When"]).ToString("yyyy-MM-dd") == AAllRebukesOnThisDay.ToString("yyyy-MM-dd")))
                        {
                            string rebukeValues = string.Empty;

                            if (!applicant.IsPersonKeyNull())
                            {
                                rebukeValues = StringHelper.AddCSV(rebukeValues, applicant.PersonKey.ToString());
                            }
                            else
                            {
                                rebukeValues = StringHelper.AddCSV(rebukeValues, applicant.PartnerKey.ToString());
                            }

                            rebukeValues = StringHelper.AddCSV(rebukeValues, applicant.FamilyName);
                            rebukeValues = StringHelper.AddCSV(rebukeValues, applicant.FirstName);
                            rebukeValues = StringHelper.AddCSV(rebukeValues, applicant.Gender);

                            Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(applicant.JSONData);

                            if (rawDataObject.Contains("JobAssigned"))
                            {
                                rebukeValues = StringHelper.AddCSV(rebukeValues, rawDataObject["JobAssigned"].ToString());
                            }
                            else
                            {
                                rebukeValues = StringHelper.AddCSV(rebukeValues, applicant.StFgCode);
                            }

                            rebukeValues = StringHelper.AddCSV(rebukeValues, applicant.StCongressCode);

                            rebukeValues =
                                StringHelper.AddCSV(rebukeValues, AAllRebukesOnThisDay.ToString("dd-MMM-yyyy") + " " +
                                    (element.Contains("Time") ? element["Time"].ToString() : string.Empty));
                            rebukeValues = StringHelper.AddCSV(rebukeValues, element["What"].ToString());
                            rebukeValues = StringHelper.AddCSV(rebukeValues, element["Consequence"].ToString());

                            string RegistrationOfficeName =
                                ((PPartnerRow)regOffices.DefaultView[regOffices.DefaultView.Find(applicant.RegistrationOffice)].Row).PartnerShortName;

                            rebukes[RegistrationOfficeName].Add(rebukeValues);
                        }
                    }
                }

                string TemplateFilename = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                    "RebukesReport",
                    "",
                    "",
                    "html");

                string ResultDocument = TFormLettersTools.PrintReport(TemplateFilename, rebukes, "Rebukes " +
                    AAllRebukesOnThisDay.ToString("dd-MMM-yyyy"), true);

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

    /// <summary>
    /// structure for the rebukes for the data grid
    /// </summary>
    public class TRebuke
    {
        /// <summary>ID, not visible</summary>
        public int ID {
            get; set;
        }
        /// <summary>Which day were the rules violated</summary>
        public DateTime When {
            get; set;
        }
        /// <summary>the time of the offence</summary>
        public string Time {
            get; set;
        }
        /// <summary>what happened</summary>
        public string What {
            get; set;
        }
        /// <summary>what will be done</summary>
        public string Consequence {
            get; set;
        }

        /// <summary>constructor</summary>
        public TRebuke(int AID)
        {
            this.ID = AID;
            this.When = DateTime.Now;
            this.Time = string.Empty;
            this.What = string.Empty;
            this.Consequence = "TBD";
        }

        /// <summary>constructor</summary>
        public TRebuke(int AID, DateTime ADate, string ATime, string AWhat, string AConsequence)
        {
            this.ID = AID;
            this.When = ADate;
            this.Time = ATime;
            this.What = AWhat;
            this.Consequence = AConsequence;
        }
    }
}