//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
    public class TConferenceCreateGiftBatch
    {
        /// <summary>
        /// get the name of this partner, and the person key from the local database from the first PmGeneralApplication for this partner
        /// </summary>
        /// <param name="ARegistrationOfficeKey"></param>
        /// <param name="ARegistrationPartnerKey"></param>
        /// <param name="ALocalPartnerKey">will be -1 if the partner has no local key yet</param>
        /// <param name="AFirstnameLastname"></param>
        /// <param name="ATransaction"></param>
        /// <returns>false if person cannot be found at all</returns>
        static private bool GetPartner(
            Int64 ARegistrationOfficeKey,
            Int64 ARegistrationPartnerKey,
            out Int64 ALocalPartnerKey,
            out String AFirstnameLastname,
            TDBTransaction ATransaction)
        {
            ALocalPartnerKey = -1;
            AFirstnameLastname = String.Empty;

            PPersonTable Person = PPersonAccess.LoadByPrimaryKey(ARegistrationPartnerKey, ATransaction);

            if (Person.Count == 0)
            {
                return false;
            }

            AFirstnameLastname = Person[0].FirstName + " " + Person[0].FamilyName;

            PmGeneralApplicationTable GeneralApplication = PmGeneralApplicationAccess.LoadViaPPersonPartnerKey(ARegistrationPartnerKey, ATransaction);

            foreach (PmGeneralApplicationRow row in GeneralApplication.Rows)
            {
                if (row.RegistrationOffice != ARegistrationOfficeKey)
                {
                    // most probably a typo? This person should belong to the registration office
                    return false;
                }
            }

            foreach (PmGeneralApplicationRow row in GeneralApplication.Rows)
            {
                if (!row.IsLocalPartnerKeyNull())
                {
                    ALocalPartnerKey = row.LocalPartnerKey;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// this is needed to create gift transactions CSV file for import into Petra 2.x
        /// </summary>
        /// <param name="AInputPartnerKeysAndPaymentInfo">CSV text with partner key and columns for payment information</param>
        /// <param name="AUnknownPartner"></param>
        /// <param name="AUnkownPartnerName"></param>
        /// <param name="ATemplateApplicationFee"></param>
        /// <param name="ATemplateManualApplication"></param>
        /// <param name="ATemplateConferenceFee"></param>
        /// <param name="ATemplateDonation"></param>
        /// <returns></returns>
        static public string CreateGiftTransactions(string AInputPartnerKeysAndPaymentInfo,
            Int64 AUnknownPartner,
            string AUnkownPartnerName,
            string ATemplateApplicationFee,
            string ATemplateManualApplication,
            string ATemplateConferenceFee,
            string ATemplateDonation)
        {
            decimal PreviousConferenceFee = 0.0m, PreviousApplicationFee = 0.0m, PreviousDonation = 0.0m, PreviousManualApplicationFee = 0.0m;
            Int64 PreviousRegistrationOffice = -1;

            string InputSeparator = ",";
            string OutputSeparator = ";";
            int RowCount = 1;
            string ResultString = string.Empty;

            if (AInputPartnerKeysAndPaymentInfo.Contains("\t"))
            {
                InputSeparator = "\t";
            }
            else if (AInputPartnerKeysAndPaymentInfo.Contains(";"))
            {
                InputSeparator = ";";
            }

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            try
            {
                string[] InputLines = AInputPartnerKeysAndPaymentInfo.Replace("\r", "").Split(new char[] { '\n' });

                foreach (string InputLine in InputLines)
                {
                    string reference = string.Empty;

                    string line = InputLine;
                    Int64 RegistrationKey = Convert.ToInt64(StringHelper.GetNextCSV(ref line, InputSeparator, ""));

                    if (RegistrationKey < 1000000)
                    {
                        RegistrationKey += 4000000;
                    }

                    decimal ConferenceFee = Convert.ToDecimal(StringHelper.GetNextCSV(ref line, InputSeparator, PreviousConferenceFee.ToString()));
                    PreviousConferenceFee = ConferenceFee;

                    decimal ApplicationFee = Convert.ToDecimal(StringHelper.GetNextCSV(ref line, InputSeparator, PreviousApplicationFee.ToString()));
                    PreviousApplicationFee = ApplicationFee;

                    decimal Donation = Convert.ToDecimal(StringHelper.GetNextCSV(ref line, InputSeparator, PreviousDonation.ToString()));
                    PreviousDonation = Donation;

                    Int64 RegistrationOffice = Convert.ToInt64(StringHelper.GetNextCSV(ref line, InputSeparator, PreviousRegistrationOffice.ToString()));
                    PreviousRegistrationOffice = RegistrationOffice;

                    decimal ManualApplicationFee = 0.0m;

                    if (line.Length > 0)
                    {
                        ManualApplicationFee =
                            Convert.ToDecimal(StringHelper.GetNextCSV(ref line, InputSeparator, PreviousManualApplicationFee.ToString()));
                        PreviousManualApplicationFee = ManualApplicationFee;
                    }

                    string PersonFirstnameLastname;
                    Int64 LocalPartnerKey;

                    if (!GetPartner(RegistrationOffice, RegistrationKey, out LocalPartnerKey, out PersonFirstnameLastname, Transaction))
                    {
                        Console.WriteLine("Cannot find partner key " + RegistrationKey.ToString() + " in row " + RowCount.ToString());
                        LocalPartnerKey = AUnknownPartner;
                        PersonFirstnameLastname = AUnkownPartnerName;

                        // we need to have a different reference, otherwise the gifts will be grouped for unknown donor, split gifts
                        reference = RowCount.ToString();
                    }
                    else if (LocalPartnerKey == -1)
                    {
                        Console.WriteLine(
                            "Problem: no person key available from Petra. " + RegistrationKey.ToString() + " in row " + RowCount.ToString());
                        LocalPartnerKey = AUnknownPartner;

                        // we need to have a different reference, otherwise the gifts will be grouped for unknown donor, split gifts
                        reference = RowCount.ToString();
                    }

                    if (ApplicationFee > 0.0m)
                    {
                        string newLine = String.Empty;

                        string template = ATemplateApplicationFee;

                        newLine = StringHelper.AddCSV(newLine, LocalPartnerKey.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, PersonFirstnameLastname, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, reference, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "<none>", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "0", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, ApplicationFee.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "no", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "yes", OutputSeparator);

                        ResultString += Environment.NewLine + newLine;
                    }

                    if (ManualApplicationFee > 0.0m)
                    {
                        string newLine = String.Empty;
                        string template = ATemplateManualApplication;

                        newLine = StringHelper.AddCSV(newLine, LocalPartnerKey.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, PersonFirstnameLastname, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, reference, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "<none>", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "0", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, ManualApplicationFee.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "no", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "yes", OutputSeparator);

                        ResultString += Environment.NewLine + newLine;
                    }

                    if (ConferenceFee > 0.0m)
                    {
                        string newLine = String.Empty;
                        string template = ATemplateConferenceFee;

                        newLine = StringHelper.AddCSV(newLine, LocalPartnerKey.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, PersonFirstnameLastname, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, reference, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "<none>", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "0", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, ConferenceFee.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "no", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "yes", OutputSeparator);

                        ResultString += Environment.NewLine + newLine;
                    }

                    if (Donation > 0.0m)
                    {
                        string newLine = String.Empty;
                        string template = ATemplateDonation;

                        // TODO: depending on amount??? assign to family key?
                        newLine = StringHelper.AddCSV(newLine, AUnknownPartner.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, AUnkownPartnerName, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "CASH", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, reference, OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "<none>", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, Donation.ToString(), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "no", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, StringHelper.GetNextCSV(ref template), OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "Both", OutputSeparator);
                        newLine = StringHelper.AddCSV(newLine, "yes", OutputSeparator);

                        ResultString += Environment.NewLine + newLine;
                    }

                    RowCount++;
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultString;
        }
    }
}