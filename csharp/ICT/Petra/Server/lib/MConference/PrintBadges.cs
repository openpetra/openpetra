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
    /// create badges for the participants of a conference
    /// </summary>
    public class TConferenceBadges
    {
        private static PUnitTable Units = null;

        private static string FormatBadge(ConferenceApplicationTDS AMainDS, ConferenceApplicationTDSApplicationGridRow AApplicant)
        {
            if (TLogging.DebugLevel > 5)
            {
                TLogging.Log("Printing badge " + AApplicant.PartnerKey.ToString());
            }

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

            Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(TJsonTools.RemoveContainerControls(AApplicant.JSONData));

            HTMLText = HTMLText.Replace("#FORMLETTERPATH", TAppSettingsManager.GetValue("Formletters.Path"));
            HTMLText = HTMLText.Replace("#ROLE", AApplicant.StCongressCode);

            string FirstName = AApplicant.FirstName;

            if (rawDataObject.Contains("NickName") && (rawDataObject["NickName"].ToString().Trim().Length > 0))
            {
                FirstName = rawDataObject["NickName"].ToString();
            }

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

            if (AApplicant.StFgCode.Length == 0)
            {
                // eg: we need a fellowship group code for teenagers and coaches
                if (RolesThatRequireFellowshipGroupCode.Contains(AApplicant.StCongressCode + ","))
                {
                    TLogging.Log("badge has no FG: " + AApplicant.PartnerKey.ToString());
                    return string.Empty;
                }
            }

            HTMLText = HTMLText.Replace("#FELLOWSHIPGROUP", AApplicant.StFgCode);

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
            if (!TFormLettersTools.CheckImagesFileExist(HTMLText))
            {
                // CheckImagesFileExist does some logging
                return string.Empty;
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
            // we want to avoid that all badges get printed by accident
            if (ADoNotReprint && ((ASelectedRole == null) || (ASelectedRole.Length == 0)))
            {
                TLogging.Log("PrintBadges: only per role");
                return string.Empty;
            }

            try
            {
                TAttendeeManagement.RefreshAttendees(AEventPartnerKey, AEventCode);

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

                    if (AttendeeRow.BadgePrint.HasValue)
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
                        CountPrinted++;
                    }
                }

                const Int32 MAXPRINTALLBADGES = 500;

                if ((CountPrinted > MAXPRINTALLBADGES) && ADoNotReprint
                    && ((ASelectedRegistrationOffice == -1) || (ASelectedRole == null) || (ASelectedRole.Length == 0)))
                {
                    TLogging.Log(
                        String.Format("PrintBadges: if more than {0} badges, print only per role and registration office", MAXPRINTALLBADGES));
                    return string.Empty;
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