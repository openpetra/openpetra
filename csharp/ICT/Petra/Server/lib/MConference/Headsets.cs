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
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
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
    /// useful functions for handing out and collecting headsets for specific sessions during a conference
    /// </summary>
    public class THeadsetManagement
    {
        private static string SESSION_CONTACT_ATTRIBUTE = "SESSION";
        private static string HEADSET_OUT_METHOD_OF_CONTACT = "HEADSET_OUT";
        private static string HEADSET_RETURN_METHOD_OF_CONTACT = "HEADSET_RETURN";

        /// <summary>
        /// get the sessions available for headset usage
        /// </summary>
        public static PContactAttributeDetailTable GetSessions()
        {
            return PContactAttributeDetailAccess.LoadViaPContactAttribute(SESSION_CONTACT_ATTRIBUTE, null);
        }

        /// <summary>
        /// create a new sessions where the participants can use their headset
        /// </summary>
        public static void AddSession(string ASessionName)
        {
            if (ASessionName.Trim().Length == 0)
            {
                return;
            }

            PContactAttributeDetailTable table = new PContactAttributeDetailTable();
            PContactAttributeDetailRow row = table.NewRowTyped(true);
            row.ContactAttributeCode = SESSION_CONTACT_ATTRIBUTE;
            row.ContactAttrDetailCode = ASessionName;
            row.ContactAttrDetailDescr = ASessionName;
            table.Rows.Add(row);

            TDBTransaction writeTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            TVerificationResultCollection VerificationResult;
            table.ThrowAwayAfterSubmitChanges = true;

            if (PContactAttributeDetailAccess.SubmitChanges(table, writeTransaction, out VerificationResult))
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// add partner key from the scanner of the badges
        /// </summary>
        public static bool AddScannedKeys(string ASessionName, string APartnerKeys, bool AHandingOutHeadset)
        {
            if (APartnerKeys.Trim().Length == 0)
            {
                return true;
            }

            ContactTDS MainDS = new ContactTDS();

            string[] InputLines = APartnerKeys.Replace("\r", "").Split(new char[] { '\n' });

            foreach (string InputLine in InputLines)
            {
                PPartnerContactRow PartnerContactRow = MainDS.PPartnerContact.NewRowTyped(true);
                PartnerContactRow.ContactId = (MainDS.PPartnerContact.Rows.Count + 1) * -1;
                PartnerContactRow.PartnerKey = Convert.ToInt64(InputLine);
                PartnerContactRow.ContactCode = (AHandingOutHeadset ? HEADSET_OUT_METHOD_OF_CONTACT : HEADSET_RETURN_METHOD_OF_CONTACT);
                PartnerContactRow.ContactDate = DateTime.Now;
                PartnerContactRow.ContactTime = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalSeconds);
                MainDS.PPartnerContact.Rows.Add(PartnerContactRow);

                PPartnerContactAttributeRow PartnerContactAttributeRow = MainDS.PPartnerContactAttribute.NewRowTyped(true);
                PartnerContactAttributeRow.ContactId = PartnerContactRow.ContactId;
                PartnerContactAttributeRow.ContactAttributeCode = SESSION_CONTACT_ATTRIBUTE;
                PartnerContactAttributeRow.ContactAttrDetailCode = ASessionName;
                MainDS.PPartnerContactAttribute.Rows.Add(PartnerContactAttributeRow);
            }

            TVerificationResultCollection VerificationResult;
            MainDS.ThrowAwayAfterSubmitChanges = true;
            return TSubmitChangesResult.scrOK == ContactTDSAccess.SubmitChanges(MainDS, out VerificationResult);
        }

        /// <summary>
        /// get Excel file with unreturned headsets
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool DownloadUnreturnedHeadsets(MemoryStream m)
        {
            return false;
        }
    }
}