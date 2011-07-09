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
    /// provides methods for managing the attendees
    /// </summary>
    public class TAttendeeManagement
    {
        /// <summary>
        /// Load/Refresh Attendees
        /// </summary>
        public static void RefreshAttendees(Int64 AConferenceKey, string AEventCode)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            PcConferenceTable ConferenceTable = new PcConferenceTable();
            string OutreachPrefix = String.Empty;

            try
            {
                // Find the conference that has the beginning 5 chars(pre-fix)
                //   we need for finding all related conferences
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, Transaction);

                if (ConferenceTable.Count == 0)
                {
                    throw new Exception("Cannot find conference " + AConferenceKey.ToString());
                }

                OutreachPrefix = ConferenceTable[0].OutreachPrefix;

                // get all conference that are related to the given one
                PcConferenceRow templateConferenceRow = ConferenceTable.NewRowTyped(false);
                templateConferenceRow.OutreachPrefix = OutreachPrefix;

                ConferenceTable = PcConferenceAccess.LoadUsingTemplate(templateConferenceRow, Transaction);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            // Run over all conference that are related to the given one
            foreach (PcConferenceRow ConferenceRow in ConferenceTable.Rows)
            {
                // get all applications for this conference
                ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();
                TApplicationManagement.GetApplications(
                    ref MainDS,
                    ConferenceRow.ConferenceKey,
                    AEventCode, "all", -1, null, false);

                foreach (PmShortTermApplicationRow ShortTermAppRow in MainDS.PmShortTermApplication.Rows)
                {
                    if (!IsAttendeeValid(MainDS, ConferenceRow.ConferenceKey, ShortTermAppRow.PartnerKey))
                    {
                        // ignore deleted applications, or cancelled applications
                        continue;
                    }

                    // Do we have a record for this attendee yet?
                    if (MainDS.PcAttendee.DefaultView.Find(ShortTermAppRow.PartnerKey) == -1)
                    {
                        PcAttendeeRow AttendeeRow = MainDS.PcAttendee.NewRowTyped();
                        AttendeeRow.ConferenceKey = ConferenceRow.ConferenceKey;
                        AttendeeRow.PartnerKey = ShortTermAppRow.PartnerKey;

                        DataView GenAppView = MainDS.PmGeneralApplication.DefaultView;
                        int GenAppIndex = GenAppView.Find(new object[] { ShortTermAppRow.PartnerKey, ShortTermAppRow.ApplicationKey,
                                                                         ShortTermAppRow.RegistrationOffice });

                        PmGeneralApplicationRow GeneralAppRow = (PmGeneralApplicationRow)GenAppView[GenAppIndex].Row;

                        DateTime DateAccepted = GeneralAppRow.GenAppDate;

                        if (!GeneralAppRow.IsGenAppSendFldAcceptDateNull())
                        {
                            DateAccepted = GeneralAppRow.GenAppSendFldAcceptDate.Value;
                        }
                        else if (!GeneralAppRow.IsGenAppRecvgFldAcceptNull())
                        {
                            DateAccepted = GeneralAppRow.GenAppRecvgFldAccept.Value;
                        }

                        AttendeeRow.Registered = DateAccepted;

                        // TODO: in Petra 2.x, this was calculated from pm_staff_data, or from the partner key / 1000000
                        AttendeeRow.HomeOfficeKey = ShortTermAppRow.RegistrationOffice;

                        MainDS.PcAttendee.Rows.Add(AttendeeRow);
                    }
                }

                // now check the other way: all attendees of this conference, are they still valid?
                foreach (PcAttendeeRow AttendeeRow in MainDS.PcAttendee.Rows)
                {
                    if ((AttendeeRow.RowState != DataRowState.Added)
                        && !IsAttendeeValid(MainDS, ConferenceRow.ConferenceKey, AttendeeRow.PartnerKey))
                    {
                        // TODO: delete pc_room_alloc
                        // TODO: delete pc_extra_cost

                        AttendeeRow.Delete();
                    }
                }

                TVerificationResultCollection VerificationResult;
                ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);

                TLogging.Log(String.Format(
                        "RefreshAttendees: finished. OutreachPrefix: {0}, {1} Shortterm Applications, {2} Attendees",
                        OutreachPrefix, MainDS.PmShortTermApplication.Count, MainDS.PcAttendee.Count));
            }
        }

        /// <summary>
        /// if attendee is not valid anymore, the attendee should be removed from pc_attendee table
        /// </summary>
        /// <returns></returns>
        private static bool IsAttendeeValid(ConferenceApplicationTDS AMainDS,
            Int64 AConferenceKey,
            Int64 AAttendeeKey)
        {
            DataView ShortTermView = AMainDS.PmShortTermApplication.DefaultView;

            int ShortTermIndex = ShortTermView.Find(new object[] { AConferenceKey, AAttendeeKey });

            if (ShortTermIndex == -1)
            {
                return false;
            }

            PmShortTermApplicationRow ShortTermRow = (PmShortTermApplicationRow)ShortTermView[ShortTermIndex].Row;

            if (ShortTermRow.StBasicDeleteFlag)
            {
                return false;
            }

            DataView GenAppView = AMainDS.PmGeneralApplication.DefaultView;

            int GenAppIndex = GenAppView.Find(new object[] { ShortTermRow.PartnerKey, ShortTermRow.ApplicationKey, ShortTermRow.RegistrationOffice });

            if (GenAppIndex == -1)
            {
                return false;
            }

            PmGeneralApplicationRow GeneralAppRow = (PmGeneralApplicationRow)GenAppView[GenAppIndex].Row;

            if (GeneralAppRow.GenAppDeleteFlag)
            {
                return false;
            }

            // for the moment, we only want to deal with accepted registrations.
            // The online registration puts people on Hold by default, which could cause confusion here
            // if (!(GeneralAppRow.GenApplicationStatus.StartsWith("H") || GeneralAppRow.GenApplicationStatus.StartsWith("A")))
            if (!GeneralAppRow.GenApplicationStatus.StartsWith("A"))
            {
                return false;
            }

            return true;
        }

        private static SortedList <Int64, DateTime>TShirtDeadLines = null;

        private static bool AcceptedBeforeTShirtDeadLine(PcAttendeeRow AAttendeeRow, ConferenceApplicationTDSApplicationGridRow AApplicant)
        {
            if (TShirtDeadLines == null)
            {
                TShirtDeadLines = new SortedList <Int64, DateTime>();

                // load T-Shirt Deadlines from text file
                // format: Partnerkey of Registration office, year, month, day
                // key 0 is the default date

                if (!TAppSettingsManager.HasValue("ConferenceTool.TShirtDeadlines.Path"))
                {
                    throw new Exception("Cannot find ConferenceTool.TShirtDeadlines.Path in config file");
                }

                StreamReader sr = new StreamReader(TAppSettingsManager.GetValue("ConferenceTool.TShirtDeadlines.Path"));

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (!line.Trim().StartsWith("#"))
                    {
                        TShirtDeadLines.Add(Convert.ToInt64(StringHelper.GetNextCSV(ref line)),
                            new DateTime(Convert.ToInt32(StringHelper.GetNextCSV(ref line)),
                                Convert.ToInt32(StringHelper.GetNextCSV(ref line)),
                                Convert.ToInt32(StringHelper.GetNextCSV(ref line))));
                    }
                }
            }

            DateTime DeadLine = TShirtDeadLines[0];

            if (TShirtDeadLines.ContainsKey(AApplicant.RegistrationOffice))
            {
                DeadLine = TShirtDeadLines[AApplicant.RegistrationOffice];
            }

            return AAttendeeRow.Registered.Value.CompareTo(DeadLine) <= 0;
        }

        /// <summary>
        /// we need to know how many applicants from which country will get a T-Shirt,
        /// using the same algorithm as the badge printing procedure
        /// </summary>
        /// <param name="AConferenceKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="AStream">write the Excel file into this stream</param>
        public static bool DownloadTShirtNumbers(Int64 AConferenceKey, string AEventCode, MemoryStream AStream)
        {
            // get all applications for this conference
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            TApplicationManagement.GetApplications(ref MainDS, AConferenceKey, AEventCode, "all", -1, null, false);

            // count the T-Shirts
            SortedList <string, Int32>TShirtCountPerCountry = new SortedList <string, int>();
            SortedList <string, Int32>TShirtCount = new SortedList <string, int>();
            MainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetPartnerKeyDBName();

            foreach (PcAttendeeRow attendee in MainDS.PcAttendee.Rows)
            {
                ConferenceApplicationTDSApplicationGridRow applicant =
                    (ConferenceApplicationTDSApplicationGridRow)MainDS.ApplicationGrid.DefaultView[MainDS.ApplicationGrid.DefaultView.Find(attendee
                                                                                                       .PartnerKey)].Row;

                Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(TJsonTools.RemoveContainerControls(applicant.JSONData));

                if (!rawDataObject.Contains("TShirtSize") || !rawDataObject.Contains("TShirtStyle"))
                {
                    continue;
                }

                if (AcceptedBeforeTShirtDeadLine(attendee, applicant))
                {
                    string TShirtId = applicant.RegistrationOffice.ToString() + ", " +
                                      rawDataObject["TShirtStyle"].ToString() + ", " +
                                      rawDataObject["TShirtSize"].ToString();

                    if (TShirtCountPerCountry.ContainsKey(TShirtId))
                    {
                        TShirtCountPerCountry[TShirtId]++;
                    }
                    else
                    {
                        TShirtCountPerCountry.Add(TShirtId, 1);
                    }

                    TShirtId = "Total," + rawDataObject["TShirtStyle"].ToString() + ", " +
                               rawDataObject["TShirtSize"].ToString();

                    if (TShirtCount.ContainsKey(TShirtId))
                    {
                        TShirtCount[TShirtId]++;
                    }
                    else
                    {
                        TShirtCount.Add(TShirtId, 1);
                    }
                }
            }

            // write the result to an Excel file
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            foreach (string key in TShirtCountPerCountry.Keys)
            {
                XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                myDoc.DocumentElement.AppendChild(newNode);
                XmlAttribute attr;

                int Counter = 1;
                string list = key;

                while (list.Length > 0)
                {
                    attr = myDoc.CreateAttribute("column" + Counter.ToString());
                    attr.Value = StringHelper.GetNextCSV(ref list);
                    newNode.Attributes.Append(attr);
                    Counter++;
                }

                attr = myDoc.CreateAttribute("columnCount");
                attr.Value = new TVariant(TShirtCountPerCountry[key]).EncodeToString();
                newNode.Attributes.Append(attr);
            }

            foreach (string key in TShirtCount.Keys)
            {
                XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                myDoc.DocumentElement.AppendChild(newNode);
                XmlAttribute attr;

                int Counter = 1;
                string list = key;

                while (list.Length > 0)
                {
                    attr = myDoc.CreateAttribute("column" + Counter.ToString());
                    attr.Value = StringHelper.GetNextCSV(ref list);
                    newNode.Attributes.Append(attr);
                    Counter++;
                }

                attr = myDoc.CreateAttribute("columnCount");
                attr.Value = new TVariant(TShirtCount[key]).EncodeToString();
                newNode.Attributes.Append(attr);
            }

            return TCsv2Xml.Xml2ExcelStream(myDoc, AStream);
        }

        private static PUnitTable Units = null;

        private static string FormatBadge(ConferenceApplicationTDS AMainDS, ConferenceApplicationTDSApplicationGridRow AApplicant)
        {
            TLogging.Log(AApplicant.PartnerKey.ToString());
            string FileName = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                "Badge",
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

            HTMLText = HTMLText.Replace("#FORMLETTERPATH", TAppSettingsManager.GetValue("Formletters.Path"));
            HTMLText = HTMLText.Replace("#FIRSTNAME", AApplicant.FirstName);
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

            HTMLText = HTMLText.Replace("#COUNTRY", ((PUnitRow)Units.DefaultView[Units.DefaultView.Find(
                                                                                     AApplicant.RegistrationOffice)].Row).UnitName);

            if (!AApplicant.IsPersonKeyNull())
            {
                HTMLText = HTMLText.Replace("#PERSONKEY", AApplicant.PersonKey.ToString());
            }
            else
            {
                HTMLText = HTMLText.Replace("#PERSONKEY", AApplicant.PartnerKey.ToString());
            }

            HTMLText = HTMLText.Replace("#BARCODEKEY", AApplicant.PartnerKey.ToString());

            string PhotoPath = TAppSettingsManager.GetValue("Server.PathData") +
                               Path.DirectorySeparatorChar + "photos" +
                               Path.DirectorySeparatorChar + AApplicant.PartnerKey.ToString() + ".jpg";

            if (!File.Exists(PhotoPath))
            {
                // don't print the badge if there is no photo
                TLogging.Log("badge has no photo: " + AApplicant.PartnerKey.ToString());
                return string.Empty;
            }

            HTMLText = HTMLText.Replace("#PHOTOPARTICIPANT", PhotoPath);

            string RolesThatRequireFellowshipGroupCode = TAppSettingsManager.GetValue("ConferenceTool.RolesThatRequireFellowshipGroupCode");
            RolesThatRequireFellowshipGroupCode = RolesThatRequireFellowshipGroupCode.Replace(" ", "") + ",";

            if (RolesThatRequireFellowshipGroupCode.Contains(AApplicant.StCongressCode + ","))
            {
                // eg: we need a fellowship group code for teenagers and coaches
                if (AApplicant.StFgCode.Length == 0)
                {
                    TLogging.Log("badge has no FG: " + AApplicant.PartnerKey.ToString());
                    return string.Empty;
                }

                HTMLText = HTMLText.Replace("#FELLOWSHIPGROUP", AApplicant.StFgCode);
            }

            Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(TJsonTools.RemoveContainerControls(AApplicant.JSONData));

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

                PcAttendeeRow AttendeeRow =
                    (PcAttendeeRow)AMainDS.PcAttendee.DefaultView[AMainDS.PcAttendee.DefaultView.Find(AApplicant.PartnerKey)].Row;

                // TShirt only for applicants who have registered before the TShirt deadline
                if (AcceptedBeforeTShirtDeadLine(AttendeeRow, AApplicant))
                {
                    HTMLText = HTMLText.Replace("#TSHIRT", tsstyle + " " + tssize);
                }
                else
                {
                    HTMLText = HTMLText.Replace("#TSHIRT", string.Empty);
                }
            }

            return HTMLText;
        }

        /// <summary>
        /// generate a PDF from an HTML Document, can contain several pages
        /// </summary>
        /// <param name="AHTMLDoc"></param>
        /// <returns>path of the temporary PDF file</returns>
        private static string GeneratePDFFromHTML(string AHTMLDoc)
        {
            if (AHTMLDoc.Length == 0)
            {
                return string.Empty;
            }

            string pdfPath = TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                             "badges";

            if (!Directory.Exists(pdfPath))
            {
                Directory.CreateDirectory(pdfPath);
            }

            Random rand = new Random();
            string filename = string.Empty;

            do
            {
                filename = pdfPath + Path.DirectorySeparatorChar +
                           rand.Next(1, 1000000).ToString() + ".pdf";
            } while (File.Exists(filename));

            TLogging.Log(filename);

            StreamWriter sw = new StreamWriter(filename.Replace(".pdf", ".txt"));
            sw.WriteLine(AHTMLDoc);
            sw.Close();

            try
            {
                PrintDocument doc = new PrintDocument();

                TPdfPrinter pdfPrinter = new TPdfPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
                TPrinterHtml htmlPrinter = new TPrinterHtml(AHTMLDoc,
                    String.Empty,
                    pdfPrinter);

                pdfPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

                pdfPrinter.SavePDF(filename);
            }
            catch (Exception e)
            {
                TLogging.Log("Exception while printing badge: " + e.Message);
                TLogging.Log(e.StackTrace);
                throw e;
            }

            return filename;
        }

        /// <summary>
        /// print the badges, using HTML template files
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="ASelectedRegistrationOffice"></param>
        /// <param name="ASelectedRole"></param>
        /// <param name="ADoNotReprint"></param>
        public static string PrintBadges(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ASelectedRegistrationOffice,
            string ASelectedRole,
            bool ADoNotReprint)
        {
            try
            {
                RefreshAttendees(AEventPartnerKey, AEventCode);

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
                    ConferenceApplicationTDSApplicationGridTable.GetStCongressCodeDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

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

                    if (ADoNotReprint && AttendeeRow.BadgePrint.HasValue)
                    {
                        // check if this badge has been printed already
                        // skip the current badge
                        continue;
                    }

                    // create an HTML file using the template files
                    bool BatchPrinted = TFormLettersTools.AttachNextPage(ref ResultDocument, FormatBadge(MainDS, applicant));

                    if (BatchPrinted)
                    {
                        AttendeeRow.BadgePrint = DatePrinted;
                    }
                }

                TFormLettersTools.CloseDocument(ref ResultDocument);

                if (ResultDocument.Length == 0)
                {
                    return String.Empty;
                }

                string PDFPath = GeneratePDFFromHTML(ResultDocument);

                if (ADoNotReprint)
                {
                    // store modified date printed for badges
                    TVerificationResultCollection VerificationResult;
                    ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);
                }

                return PDFPath;
            }
            catch (Exception e)
            {
                TLogging.Log("Exception while printing badges: " + e.Message);
                TLogging.Log(e.StackTrace);
                throw e;
            }
        }
    }
}