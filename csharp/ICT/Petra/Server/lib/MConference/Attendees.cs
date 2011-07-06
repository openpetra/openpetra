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
                ConferenceApplicationTDS MainDS = TApplicationManagement.GetApplications(AEventCode, "all", -1, null);

                // required for DefaultView.Find
                MainDS.PmShortTermApplication.DefaultView.Sort =
                    PmShortTermApplicationTable.GetStConfirmedOptionDBName() + "," +
                    PmShortTermApplicationTable.GetPartnerKeyDBName();
                MainDS.PmGeneralApplication.DefaultView.Sort =
                    PmGeneralApplicationTable.GetPartnerKeyDBName() + "," +
                    PmGeneralApplicationTable.GetApplicationKeyDBName() + "," +
                    PmGeneralApplicationTable.GetRegistrationOfficeDBName();

                LoadAttendees(ref MainDS, ConferenceRow.ConferenceKey);

                foreach (PmShortTermApplicationRow ShortTermAppRow in MainDS.PmShortTermApplication.Rows)
                {
                    if (!IsAttendeeValid(MainDS, ConferenceRow.ConferenceKey, ShortTermAppRow.PartnerKey))
                    {
                        // ignore deleted applications, or cancelled applications
                        continue;
                    }

                    // Do we have a record for this attendee yet?
                    if (MainDS.PcAttendee.DefaultView.Find(new object[] { ConferenceRow.ConferenceKey, ShortTermAppRow.PartnerKey }) == -1)
                    {
                        PcAttendeeRow AttendeeRow = MainDS.PcAttendee.NewRowTyped();
                        AttendeeRow.ConferenceKey = ConferenceRow.ConferenceKey;
                        AttendeeRow.PartnerKey = ShortTermAppRow.PartnerKey;

                        DataView GenAppView = MainDS.PmGeneralApplication.DefaultView;
                        int GenAppIndex = GenAppView.Find(new object[] { ShortTermAppRow.PartnerKey, ShortTermAppRow.ApplicationKey,
                                                                         ShortTermAppRow.RegistrationOffice });

                        PmGeneralApplicationRow GeneralAppRow = (PmGeneralApplicationRow)GenAppView[GenAppIndex].Row;

                        AttendeeRow.Registered = GeneralAppRow.GenAppSendFldAcceptDate;

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
        /// load all attendees of this conference
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AConferenceKey"></param>
        private static void LoadAttendees(ref ConferenceApplicationTDS AMainDS, Int64 AConferenceKey)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            try
            {
                // load all attendees of this conference
                AMainDS.PcAttendee.Clear();
                PcAttendeeRow templateAttendeeRow = AMainDS.PcAttendee.NewRowTyped(false);
                templateAttendeeRow.ConferenceKey = AConferenceKey;
                PcAttendeeAccess.LoadUsingTemplate(AMainDS, templateAttendeeRow, Transaction);
                AMainDS.PcAttendee.DefaultView.Sort = PcAttendeeTable.GetConferenceKeyDBName() + ", " + PcAttendeeTable.GetPartnerKeyDBName();
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
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

        private static PUnitTable Units = null;

        private static string FormatBadge(ConferenceApplicationTDSApplicationGridRow AApplicant)
        {
            string FileName = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                "Badge",
                "",
                AApplicant.StCongressCode,
                "html");

            if (!File.Exists(FileName))
            {
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
                                                                                     AApplicant.RegistrationOffice)].Row).Description);

            if (AApplicant.PersonKey > 0)
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
                return string.Empty;
            }

            // TODO: scale photo, so that the PDF does not get that big. just a temp file, do not modify the original file.
            // should this happen in the PDF printer?
            HTMLText = HTMLText.Replace("#PHOTOPARTICIPANT", PhotoPath);

            return HTMLText;
        }

        /// <summary>
        /// generate a PDF from an HTML Document, can contain several pages
        /// </summary>
        /// <param name="AHTMLDoc"></param>
        /// <returns>path of the temporary PDF file</returns>
        private static string GeneratePDFFromHTML(string AHTMLDoc)
        {
            PrintDocument doc = new PrintDocument();

            TPdfPrinter pdfPrinter = new TPdfPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(AHTMLDoc,
                String.Empty,
                pdfPrinter);

            pdfPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

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

            pdfPrinter.SavePDF(filename);

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
        public static void PrintBadges(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ASelectedRegistrationOffice,
            string ASelectedRole,
            bool ADoNotReprint)
        {
            RefreshAttendees(AEventPartnerKey, AEventCode);

            ConferenceApplicationTDS MainDS = TApplicationManagement.GetApplications(AEventCode,
                "accepted",
                ASelectedRegistrationOffice,
                ASelectedRole);

            LoadAttendees(ref MainDS, AEventPartnerKey);

            // we want one timestamp for the whole batch of badges. this makes it easier to reverse/reprint if we must.
            DateTime DatePrinted = DateTime.Now;
            string ResultDocument = string.Empty;

            // go through all accepted applicants
            foreach (ConferenceApplicationTDSApplicationGridRow applicant in MainDS.ApplicationGrid.Rows)
            {
                int AttendeeIndex = MainDS.PcAttendee.DefaultView.Find(new Object[] { AEventPartnerKey, applicant.PartnerKey });

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
                bool BatchPrinted = TFormLettersTools.AttachNextPage(ref ResultDocument, FormatBadge(applicant));

                if (BatchPrinted)
                {
                    AttendeeRow.BadgePrint = DatePrinted;
                }
            }

            TFormLettersTools.CloseDocument(ref ResultDocument);

            string PDFPath = GeneratePDFFromHTML(ResultDocument);

            if (ADoNotReprint)
            {
                // store modified date printed for badges
                TVerificationResultCollection VerificationResult;
                ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);
            }
        }
    }
}