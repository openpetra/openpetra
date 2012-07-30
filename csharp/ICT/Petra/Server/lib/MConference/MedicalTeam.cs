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
using Ict.Petra.Shared;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// reports for medical team
    /// </summary>
    public class TMedicalReport
    {
        private static string MODULE_MEDICAL = "MEDICAL";

        /// <summary>
        /// print all incidents of one day
        /// </summary>
        public static string PrintReport(Int64 AEventPartnerKey, string AEventCode, DateTime ADateToPrint)
        {
            if (!UserInfo.GUserInfo.IsInModule(MODULE_MEDICAL))
            {
                return string.Empty;
            }

            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

            try
            {
                ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

                TApplicationManagement.GetApplications(
                    ref MainDS,
                    AEventPartnerKey,
                    AEventCode,
                    "accepted",
                    -1,
                    null,
                    false);

                PPartnerTable offices = TApplicationManagement.GetRegistrationOffices();

                MainDS.ApplicationGrid.DefaultView.Sort =
                    ConferenceApplicationTDSApplicationGridTable.GetRegistrationOfficeDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

                SortedList <string, List <string>>incidentsPerPartner = new SortedList <string, List <string>>();

                // go through all applicants
                foreach (DataRowView rv in MainDS.ApplicationGrid.DefaultView)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant = (ConferenceApplicationTDSApplicationGridRow)rv.Row;

                    string MedicalLogs = TMedicalLogs.GetMedicalLogs(MainDS, applicant.PartnerKey);

                    string groupname = applicant.FirstName + " " + applicant.FamilyName + " " + applicant.PartnerKey.ToString();

                    if (MedicalLogs.Length > 0)
                    {
                        Jayrock.Json.JsonArray list;

                        try
                        {
                            list = (Jayrock.Json.JsonArray)Jayrock.Json.Conversion.JsonConvert.Import(
                                TJsonTools.RemoveContainerControls(MedicalLogs));
                        }
                        catch (Exception)
                        {
                            TLogging.Log(MedicalLogs);
                            throw;
                        }

                        foreach (Jayrock.Json.JsonObject element in list)
                        {
                            DateTime DateOfIncident = DateTime.Today;

                            try
                            {
                                DateOfIncident = Convert.ToDateTime(element["dtpDate"]);
                            }
                            catch (Exception)
                            {
                                TLogging.Log("cannot parse to datetime " + element["dtpDate"]);
                            }

                            if (DateOfIncident.ToString("yyyy-MM-dd") == ADateToPrint.ToString("yyyy-MM-dd"))
                            {
                                if (!incidentsPerPartner.ContainsKey(groupname))
                                {
                                    incidentsPerPartner.Add(groupname, new List <string>());

                                    string CSVValuesHeader = string.Empty;
                                    CSVValuesHeader =
                                        StringHelper.AddCSV(CSVValuesHeader, StringHelper.DateToLocalizedString(applicant.DateOfBirth.Value));
                                    CSVValuesHeader = StringHelper.AddCSV(CSVValuesHeader, applicant.Gender);
                                    CSVValuesHeader =
                                        StringHelper.AddCSV(CSVValuesHeader, ((PPartnerRow)offices.Rows.Find(
                                                                                  applicant.RegistrationOffice)).PartnerShortName);
                                    Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(applicant.JSONData);

                                    string MedicalInfo = string.Empty;

                                    foreach (string key in rawDataObject.Names)
                                    {
                                        if (key.ToLower().Contains("health")
                                            || key.ToLower().Contains("emergency")
                                            || key.ToLower().Contains("medical")
                                            || key.ToLower().Contains("vegetarian"))
                                        {
                                            MedicalInfo += "<tr><th>" + key + "</th><td>" + rawDataObject[key].ToString() + "</td></tr>";
                                        }
                                    }

                                    CSVValuesHeader = StringHelper.AddCSV(CSVValuesHeader, MedicalInfo);

                                    incidentsPerPartner[groupname].Add(TFormLettersTools.HEADERGROUP + CSVValuesHeader);
                                }

                                string CSVValues = string.Empty;
                                CSVValues = StringHelper.AddCSV(CSVValues,
                                    StringHelper.DateToLocalizedString(Convert.ToDateTime(element["dtpDate"].ToString())));
                                CSVValues = StringHelper.AddCSV(CSVValues, element["txtExaminer"].ToString());
                                CSVValues = StringHelper.AddCSV(CSVValues, element["txtPulse"].ToString());
                                CSVValues = StringHelper.AddCSV(CSVValues, element["txtBloodPressure"].ToString());
                                CSVValues = StringHelper.AddCSV(CSVValues, element["txtTemperature"].ToString());
                                CSVValues = StringHelper.AddCSV(CSVValues, element["txtDiagnosis"].ToString());
                                CSVValues = StringHelper.AddCSV(CSVValues, element["txtTherapy"].ToString());
                                CSVValues = StringHelper.AddCSV(CSVValues, element["txtKeywords"].ToString());
                                incidentsPerPartner[groupname].Add(CSVValues);
                            }
                        }
                    }
                }

                string TemplateFilename = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                    "MedicalReport",
                    "",
                    "",
                    "html");

                string ResultDocument = TFormLettersTools.PrintReport(TemplateFilename, incidentsPerPartner, "All Incidents for day " +
                    StringHelper.DateToLocalizedString(ADateToPrint), true);

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
                throw ex;
            }
            finally
            {
                Catalog.SetCulture(OrigCulture);
            }
        }

        /// <summary>
        /// print all incidents of one partner
        /// </summary>
        public static string PrintReport(string AEventCode, Int64 APartnerToPrint)
        {
            if (!UserInfo.GUserInfo.IsInModule(MODULE_MEDICAL))
            {
                return string.Empty;
            }

            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

            try
            {
                SortedList <string, List <string>>incidentsPerPartner = new SortedList <string, List <string>>();

                ConferenceApplicationTDS MainDS = TApplicationManagement.LoadApplicationFromDB(AEventCode, APartnerToPrint);
                PPartnerTable offices = TApplicationManagement.GetRegistrationOffices();

                string groupname = MainDS.PPerson[0].FirstName + " " + MainDS.PPerson[0].FamilyName + " " + APartnerToPrint.ToString();
                incidentsPerPartner.Add(groupname, new List <string>());

                string CSVValuesHeader = string.Empty;
                CSVValuesHeader = StringHelper.AddCSV(CSVValuesHeader, StringHelper.DateToLocalizedString(MainDS.PPerson[0].DateOfBirth.Value));
                CSVValuesHeader = StringHelper.AddCSV(CSVValuesHeader, MainDS.PPerson[0].Gender);
                CSVValuesHeader =
                    StringHelper.AddCSV(CSVValuesHeader, ((PPartnerRow)offices.Rows.Find(
                                                              MainDS.PmShortTermApplication[0].RegistrationOffice)).PartnerShortName);
                string RawData = MainDS.PmGeneralApplication[0].RawApplicationData;
                Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(RawData);

                string MedicalInfo = string.Empty;

                foreach (string key in rawDataObject.Names)
                {
                    if (key.ToLower().Contains("health")
                        || key.ToLower().Contains("emergency")
                        || key.ToLower().Contains("medical")
                        || key.ToLower().Contains("vegetarian"))
                    {
                        MedicalInfo += "<tr><th>" + key + "</th><td>" + rawDataObject[key].ToString() + "</td></tr>";
                    }
                }

                CSVValuesHeader = StringHelper.AddCSV(CSVValuesHeader, MedicalInfo);

                incidentsPerPartner[groupname].Add(TFormLettersTools.HEADERGROUP + CSVValuesHeader);

                string MedicalLogs = TMedicalLogs.GetMedicalLogs(MainDS, APartnerToPrint);

                if (MedicalLogs.Length > 0)
                {
                    Jayrock.Json.JsonArray list;

                    try
                    {
                        list = (Jayrock.Json.JsonArray)Jayrock.Json.Conversion.JsonConvert.Import(
                            TJsonTools.RemoveContainerControls(MedicalLogs));
                    }
                    catch (Exception)
                    {
                        TLogging.Log(MedicalLogs);
                        throw;
                    }

                    foreach (Jayrock.Json.JsonObject element in list)
                    {
                        string CSVValues = string.Empty;
                        CSVValues = StringHelper.AddCSV(CSVValues,
                            StringHelper.DateToLocalizedString(Convert.ToDateTime(element["dtpDate"].ToString())));
                        CSVValues = StringHelper.AddCSV(CSVValues, element["txtExaminer"].ToString());
                        CSVValues = StringHelper.AddCSV(CSVValues, element["txtPulse"].ToString());
                        CSVValues = StringHelper.AddCSV(CSVValues, element["txtBloodPressure"].ToString());
                        CSVValues = StringHelper.AddCSV(CSVValues, element["txtTemperature"].ToString());
                        CSVValues = StringHelper.AddCSV(CSVValues, element["txtDiagnosis"].ToString());
                        CSVValues = StringHelper.AddCSV(CSVValues, element["txtTherapy"].ToString());
                        CSVValues = StringHelper.AddCSV(CSVValues, element["txtKeywords"].ToString());
                        incidentsPerPartner[groupname].Add(CSVValues);
                    }
                }

                string TemplateFilename = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                    "MedicalReport",
                    "",
                    "",
                    "html");

                string ResultDocument = TFormLettersTools.PrintReport(TemplateFilename, incidentsPerPartner, "All Incidents of patient", true);

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
                throw ex;
            }
            finally
            {
                Catalog.SetCulture(OrigCulture);
            }
        }
    }

    /// <summary>
    /// helper functions for medical logs
    /// </summary>
    public class TMedicalLogs
    {
        private static Int32 MedicalLabelID = -1;

        /// <summary>
        /// get the medical logs for one partner
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public static string GetMedicalLogs(ConferenceApplicationTDS AMainDS, Int64 APartnerKey)
        {
            if (MedicalLabelID == -1)
            {
                Int32 IndexLabelMedical = AMainDS.PDataLabel.DefaultView.Find("MedicalNotes");

                if (IndexLabelMedical != -1)
                {
                    MedicalLabelID = ((PDataLabelRow)AMainDS.PDataLabel.DefaultView[IndexLabelMedical].Row).Key;
                }
            }

            if (MedicalLabelID != -1)
            {
                int IndexLabel = AMainDS.PDataLabelValuePartner.DefaultView.Find(new object[] { MedicalLabelID, APartnerKey });

                if (IndexLabel != -1)
                {
                    return ((PDataLabelValuePartnerRow)AMainDS.PDataLabelValuePartner.DefaultView[IndexLabel].Row).ValueChar;
                }
            }

            return string.Empty;
        }
    }

    /// <summary>for each medical incident</summary>
    public class TMedicalIncident
    {
        /// <summary>identification number</summary>
        public int ID {
            get; set;
        }
        /// <summary>date of incident</summary>
        public DateTime Date {
            get; set;
        }
        /// <summary>medical person doing the examination/treatment</summary>
        public string Examiner {
            get; set;
        }
        /// <summary>pulse</summary>
        public string Pulse {
            get; set;
        }
        /// <summary>blood pressure</summary>
        public string BloodPressure {
            get; set;
        }
        /// <summary>temperature</summary>
        public string Temperature {
            get; set;
        }
        /// <summary>what is the problem</summary>
        public string Diagnosis {
            get; set;
        }
        /// <summary>what has been done or will be done</summary>
        public string Therapy {
            get; set;
        }
        /// <summary>keywords for reports</summary>
        public string Keywords {
            get; set;
        }

        /// <summary>constructor</summary>
        public TMedicalIncident(int AID)
        {
            this.ID = AID;
            this.Date = DateTime.Now;
        }

        /// <summary>constructor</summary>
        public TMedicalIncident(int AID, DateTime ADate, string AExaminer,
            string APulse,
            string ABloodPressure,
            string ATemperature,
            string ADiagnosis,
            string ATherapy,
            string AKeywords)
        {
            this.ID = AID;
            this.Date = ADate;
            this.Examiner = AExaminer;
            this.Pulse = APulse;
            this.BloodPressure = ABloodPressure;
            this.Temperature = ATemperature;
            this.Diagnosis = ADiagnosis;
            this.Therapy = ATherapy;
            this.Keywords = AKeywords;
        }
    }
}