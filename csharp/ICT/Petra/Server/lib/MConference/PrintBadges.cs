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
    /// create badges for the participants of a conference
    /// </summary>
    public class TConferenceBadges
    {
        private static PUnitTable Units = null;

        private static string FormatBadge(string ABadgeID,
            ConferenceApplicationTDS AMainDS,
            ConferenceApplicationTDSApplicationGridRow AApplicant,
            bool APrintAll)
        {
            if (TLogging.DebugLevel > 5)
            {
                TLogging.Log("Printing badge " + AApplicant.PartnerKey.ToString());
            }

            string FileName = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                ABadgeID,
                "",
                AApplicant.StCongressCode,
                "html");

            if (!File.Exists(FileName))
            {
                TLogging.Log("badge: cannot find template " + FileName);
                return string.Empty;
            }

            StreamReader r = new StreamReader(FileName);
            string HTMLText = r.ReadToEnd();
            r.Close();

            Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(AApplicant.JSONData);

            HTMLText = HTMLText.Replace("#FORMLETTERPATH", TAppSettingsManager.GetValue("Formletters.Path"));
            HTMLText = HTMLText.Replace("#ROLE", AApplicant.StCongressCode);

            string FirstName = AApplicant.FirstName;
            string NickName = AApplicant.FirstName;

            if (rawDataObject.Contains("NickName") && (rawDataObject["NickName"].ToString().Trim().Length > 0))
            {
                NickName = rawDataObject["NickName"].ToString();
            }

            HTMLText = HTMLText.Replace("#NICKNAME", NickName);
            HTMLText = HTMLText.Replace("#FIRSTNAME", FirstName);
            HTMLText = HTMLText.Replace("#LASTNAME", AApplicant.FamilyName);

            // TODO: use passport country?

            if (Units == null)
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
                try
                {
                    Units = PUnitAccess.LoadViaUUnitType("F", Transaction);
                }
                finally
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                Units.DefaultView.Sort = PUnitTable.GetPartnerKeyDBName();
            }

            HTMLText = HTMLText.Replace("#COUNTRY",
                ((PUnitRow)Units.Rows.Find(AApplicant.RegistrationOffice)).UnitName);

            HTMLText = HTMLText.Replace("#REGISTRATIONKEY", AApplicant.PartnerKey.ToString());

            if (!AApplicant.IsPersonKeyNull())
            {
                HTMLText = HTMLText.Replace("#PERSONKEY", AApplicant.PersonKey.ToString());
            }
            else
            {
                HTMLText = HTMLText.Replace("#PERSONKEY", AApplicant.PartnerKey.ToString());
            }

            string encodedBarCode = TBarCode128.Encode(AApplicant.PartnerKey.ToString());
            encodedBarCode = encodedBarCode.Replace("<", "&lt;");
            encodedBarCode = encodedBarCode.Replace(">", "&gt;");
            HTMLText = HTMLText.Replace("#BARCODEKEY", encodedBarCode);

            string PhotoPath = TAppSettingsManager.GetValue("Server.PathData") +
                               Path.DirectorySeparatorChar + "photos" +
                               Path.DirectorySeparatorChar + AApplicant.PartnerKey.ToString() + ".jpg";

            if (!File.Exists(PhotoPath))
            {
                if (APrintAll)
                {
                    HTMLText = HTMLText.Replace("#PHOTOPARTICIPANT", "");
                }
                else
                {
                    // don't print the badge if there is no photo
                    TLogging.Log("badge has no photo: " + AApplicant.PartnerKey.ToString());
                    return string.Empty;
                }
            }
            else
            {
                HTMLText = HTMLText.Replace("#PHOTOPARTICIPANT", PhotoPath);
            }

            string RolesThatRequireFellowshipGroupCode = TAppSettingsManager.GetValue("ConferenceTool.RolesThatRequireFellowshipGroupCode", "", false);
            RolesThatRequireFellowshipGroupCode = RolesThatRequireFellowshipGroupCode.Replace(" ", "") + ",";

            if ((AApplicant.StFgCode.Length == 0) && !APrintAll)
            {
                // eg: we need a fellowship group code for teenagers and coaches
                if (RolesThatRequireFellowshipGroupCode.Contains(AApplicant.StCongressCode + ","))
                {
                    TLogging.Log("badge has no FG: " + AApplicant.PartnerKey.ToString());
                    return string.Empty;
                }
            }

            HTMLText = HTMLText.Replace("#FELLOWSHIPGROUP", AApplicant.StFgCode);

            if (rawDataObject.Contains("JobAssigned"))
            {
                HTMLText = HTMLText.Replace("#JOBASSIGNED", rawDataObject["JobAssigned"].ToString());
            }
            else
            {
                HTMLText = HTMLText.Replace("#JOBASSIGNED", "");
            }

            if (rawDataObject.Contains("TShirtSize") && rawDataObject.Contains("TShirtStyle"))
            {
                string tsstyle = rawDataObject["TShirtStyle"].ToString();

                if (tsstyle.IndexOf("(") != -1)
                {
                    tsstyle = tsstyle.Substring(0, tsstyle.IndexOf("(") - 1);
                }

                string tssize = rawDataObject["TShirtSize"].ToString();

                if (tssize.IndexOf("(") != -1)
                {
                    tssize = tssize.Substring(0, tssize.IndexOf("(") - 1);
                }

                int indexAttendee = AMainDS.PcAttendee.DefaultView.Find(AApplicant.PartnerKey);

                PcAttendeeRow AttendeeRow =
                    (PcAttendeeRow)AMainDS.PcAttendee.DefaultView[indexAttendee].Row;

                // TShirt only for applicants who have registered before the TShirt deadline
                if (TConferenceFreeTShirt.AcceptedBeforeTShirtDeadLine(AttendeeRow, AApplicant))
                {
                    HTMLText = HTMLText.Replace("#TSHIRT", tsstyle + " " + tssize);
                }
                else
                {
                    HTMLText = HTMLText.Replace("#TSHIRT", string.Empty);
                }
            }
            else
            {
                HTMLText = HTMLText.Replace("#TSHIRT", string.Empty);
            }

            // check for all image paths, if the images actually exist
            if (!APrintAll && !TFormLettersTools.CheckImagesFileExist(HTMLText))
            {
                // CheckImagesFileExist does some logging
                return string.Empty;
            }

            return HTMLText;
        }

        /// <summary>
        /// print the badges, using HTML template files
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="ASelectedRegistrationOffice"></param>
        /// <param name="ASelectedRole"></param>
        /// <param name="AReprintPrinted">Reprint badges that have already been printed</param>
        /// <param name="ADoNotReprint">Store the current date so that badges will not be printed again</param>
        public static string PrintBadges(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ASelectedRegistrationOffice,
            string ASelectedRole,
            bool AReprintPrinted,
            bool ADoNotReprint)
        {
            // we want to avoid that all badges get printed by accident
            if (ADoNotReprint && ((ASelectedRole == null) || (ASelectedRole.Length == 0)))
            {
                TLogging.Log("PrintBadges: only per role");
                return string.Empty;
            }

            try
            {
                TAttendeeManagement.RefreshAttendees(AEventPartnerKey);

                ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();
                TApplicationManagement.GetApplications(
                    ref MainDS,
                    AEventPartnerKey,
                    AEventCode,
                    "accepted",
                    ASelectedRegistrationOffice,
                    ASelectedRole,
                    false);

                // we want one timestamp for the whole batch of badges. this makes it easier to reverse/reprint if we must.
                DateTime DatePrinted = DateTime.Now;
                string ResultDocument = string.Empty;

                MainDS.ApplicationGrid.DefaultView.Sort =
                    ConferenceApplicationTDSApplicationGridTable.GetRegistrationOfficeDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

                Int32 CountPrinted = 0;

                // go through all accepted applicants
                foreach (DataRowView rv in MainDS.ApplicationGrid.DefaultView)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant = (ConferenceApplicationTDSApplicationGridRow)rv.Row;
                    int AttendeeIndex = MainDS.PcAttendee.DefaultView.Find(applicant.PartnerKey);

                    if (AttendeeIndex == -1)
                    {
                        continue;
                    }

                    PcAttendeeRow AttendeeRow = (PcAttendeeRow)MainDS.PcAttendee.DefaultView[AttendeeIndex].Row;

                    if (AttendeeRow.BadgePrint.HasValue && !AReprintPrinted)
                    {
                        // check if this badge has been printed already
                        // skip the current badge
                        continue;
                    }

                    // create an HTML file using the template files
                    bool BatchPrinted = TFormLettersTools.AttachNextPage(ref ResultDocument, FormatBadge("Badge", MainDS, applicant, false));

                    if (BatchPrinted)
                    {
                        AttendeeRow.BadgePrint = DatePrinted;
                        CountPrinted++;
                    }

                    const Int32 MAXPRINTBADGES = 200;

                    // print maximum MAXPRINTBADGES badges at the time, otherwise the server is out of memory
                    if (!AReprintPrinted)
                    {
                        if (CountPrinted > MAXPRINTBADGES)
                        {
                            break;
                        }
                    }
                }

                TFormLettersTools.CloseDocument(ref ResultDocument);

                if (ResultDocument.Length == 0)
                {
                    TLogging.Log("there are no batches to be printed");
                    return String.Empty;
                }

                string PDFPath = TFormLettersTools.GeneratePDFFromHTML(ResultDocument,
                    TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                    "badges");

                if (ADoNotReprint)
                {
                    // store modified date printed for badges
                    MainDS.ThrowAwayAfterSubmitChanges = true;

                    ConferenceApplicationTDSAccess.SubmitChanges(MainDS);
                }

                return PDFPath;
            }
            catch (Exception e)
            {
                TLogging.Log("Exception while printing badges: " + e.Message);
                TLogging.Log(e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// reprint a badge per applicant
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="APartnerKey"></param>
        public static string ReprintBadge(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 APartnerKey)
        {
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

                int indexAttendee = MainDS.PcAttendee.DefaultView.Find(APartnerKey);

                if (indexAttendee == -1)
                {
                    TAttendeeManagement.RefreshAttendees(AEventPartnerKey);
                }

                string ResultDocument = string.Empty;

                MainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetPartnerKeyDBName();

                int PartnerIndex = MainDS.ApplicationGrid.DefaultView.Find(APartnerKey);

                if (PartnerIndex > -1)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant =
                        (ConferenceApplicationTDSApplicationGridRow)MainDS.ApplicationGrid.DefaultView[PartnerIndex].Row;

                    // create an HTML file using the template files
                    TFormLettersTools.AttachNextPage(ref ResultDocument, FormatBadge("Badge", MainDS, applicant, true));
                }

                TFormLettersTools.CloseDocument(ref ResultDocument);

                if (ResultDocument.Length == 0)
                {
                    return String.Empty;
                }

                string PDFPath = TFormLettersTools.GeneratePDFFromHTML(ResultDocument,
                    TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                    "badges");

                return PDFPath;
            }
            catch (Exception e)
            {
                TLogging.Log("Exception while reprinting badge: " + e.Message);
                TLogging.Log(e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// print the badges, using HTML template files.
        /// several badges are printed on one page
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="ASelectedRegistrationOffice"></param>
        /// <param name="ASelectedRole"></param>
        /// <param name="ATemplate">Template file in the formletters directory. without file extension html</param>
        /// <param name="APrintAll">if true, print badge even if fellowship group or photo is missing</param>
        public static string PrintBadgeLabels(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ASelectedRegistrationOffice,
            string ASelectedRole,
            string ATemplate,
            bool APrintAll)
        {
            TAttendeeManagement.RefreshAttendees(AEventPartnerKey);

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();
            TApplicationManagement.GetApplications(
                ref MainDS,
                AEventPartnerKey,
                AEventCode,
                "accepted",
                ASelectedRegistrationOffice,
                ASelectedRole,
                false);

            MainDS.ApplicationGrid.DefaultView.Sort =
                ConferenceApplicationTDSApplicationGridTable.GetRegistrationOfficeDBName() + "," +
                ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

            return PrintBadgeLabels(MainDS, ATemplate, APrintAll);
        }

        /// <summary>
        /// print the badges, using HTML template files.
        /// several badges are printed on one page.
        /// print for specified partner and person keys.
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="ASelectedPartnerKeys"></param>
        /// <param name="ATemplate">Template file in the formletters directory. without file extension html</param>
        public static string PrintBadgeLabels(Int64 AEventPartnerKey,
            string AEventCode,
            string ASelectedPartnerKeys,
            string ATemplate)
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

            string[] InputLines = ASelectedPartnerKeys.Replace("\r", "").Split(new char[] { '\n' });

            MainDS.ApplicationGrid.DefaultView.Sort =
                ConferenceApplicationTDSApplicationGridTable.GetPersonKeyDBName();

            List <Int64>Keys = new List <long>();

            foreach (string PartnerKeyString in InputLines)
            {
                Int64 Partnerkey = Convert.ToInt64(PartnerKeyString.Trim());

                int Index = MainDS.ApplicationGrid.DefaultView.Find(Partnerkey);

                if (Index != -1)
                {
                    // this is the person key. add the registration key
                    Keys.Add(((ConferenceApplicationTDSApplicationGridRow)MainDS.ApplicationGrid.DefaultView[Index].Row).PartnerKey);
                }
                else
                {
                    Keys.Add(Partnerkey);
                }
            }

            ConferenceApplicationTDSApplicationGridTable old = (ConferenceApplicationTDSApplicationGridTable)MainDS.ApplicationGrid.Copy();
            MainDS.ApplicationGrid.Clear();

            old.DefaultView.Sort =
                ConferenceApplicationTDSApplicationGridTable.GetPartnerKeyDBName();

            foreach (int key in Keys)
            {
                int Index = old.DefaultView.Find(key);

                if (Index == -1)
                {
                    TLogging.Log("cannot find " + key);
                    continue;
                }

                ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)old.DefaultView[Index].Row;

                MainDS.ApplicationGrid.ImportRow(row);
            }

            MainDS.ApplicationGrid.AcceptChanges();

            MainDS.ApplicationGrid.DefaultView.Sort =
                ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

            return PrintBadgeLabels(MainDS, ATemplate, true);
        }

        /// <summary>
        /// print the badges, using HTML template files.
        /// several badges are printed on one page.
        /// print for all applicants in the MainDS.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ATemplate">Template file in the formletters directory. without file extension html</param>
        /// <param name="APrintAll">if true, print badge even if fellowship group or photo is missing</param>
        private static string PrintBadgeLabels(
            ConferenceApplicationTDS AMainDS,
            string ATemplate,
            bool APrintAll)
        {
            try
            {
                // Load label template. this knows where to print the next label, and when to start a new page

                List <string>Labels = new List <string>();

                // go through all accepted applicants
                foreach (DataRowView rv in AMainDS.ApplicationGrid.DefaultView)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant = (ConferenceApplicationTDSApplicationGridRow)rv.Row;

                    // create an HTML file using the template files
                    string label = TFormLettersTools.GetContentsOfDiv(FormatBadge(ATemplate, AMainDS, applicant, APrintAll), "label");

                    if (label.Length > 0)
                    {
                        Labels.Add(label);
                    }
                }

                // create a title, for the footer
                string FooterTitle = string.Empty;

                if (AMainDS.ApplicationGrid.Count > 1)
                {
                    // if the first and the last applicant have same registration office, add the name to the title
                    if (AMainDS.ApplicationGrid[0].RegistrationOffice ==
                        AMainDS.ApplicationGrid[AMainDS.ApplicationGrid.Count - 1].RegistrationOffice)
                    {
                        PPartnerTable regOffices = TApplicationManagement.GetRegistrationOffices();
                        FooterTitle += " " +
                                       ((PPartnerRow)regOffices.DefaultView[regOffices.DefaultView.Find(AMainDS.ApplicationGrid[0].RegistrationOffice)
                                        ].Row).
                                       PartnerShortName;
                    }

                    // if the first and the last applicant have the same role, add the role to the title
                    if (AMainDS.ApplicationGrid[0].StCongressCode == AMainDS.ApplicationGrid[AMainDS.ApplicationGrid.Count - 1].StCongressCode)
                    {
                        FooterTitle += " " + AMainDS.ApplicationGrid[0].StCongressCode;
                    }
                }

                string BadgeLabelTemplateFilename = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                    ATemplate,
                    "",
                    "",
                    "html");

                string ResultDocument = TFormLettersTools.PrintLabels(BadgeLabelTemplateFilename, Labels, FooterTitle);

                if (ResultDocument.Length == 0)
                {
                    return String.Empty;
                }

                string PDFPath = TFormLettersTools.GeneratePDFFromHTML(ResultDocument,
                    TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                    "badges");


                return PDFPath;
            }
            catch (Exception e)
            {
                TLogging.Log("Exception while printing badge labels: " + e.Message);
                TLogging.Log(e.StackTrace);
                throw;
            }
        }
    }
}