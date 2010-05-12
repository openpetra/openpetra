//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Net;
using System.Net.Mail;
using Ict.Common.IO;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace anniversaries
{
public class TSendEmail
{
    private static string AddressToHtml(DataSet ADataSet, Int64 APartnerKey)
    {
        string htmlText = "";
        PPartnerLocationTable PartnerLocationTable = (PPartnerLocationTable)ADataSet.Tables[PPartnerLocationTable.GetTableName()];
        DataView partnerlocationview = PartnerLocationTable.DefaultView;

        partnerlocationview.Sort = "p_partner_key_n ASC";
        int indexPartnerLocation = partnerlocationview.Find(APartnerKey);

        if (indexPartnerLocation != -1)
        {
            PPartnerLocationRow PartnerLocationRow = (PPartnerLocationRow)partnerlocationview[indexPartnerLocation].Row;
            htmlText += String.Format("<td>{0}</td>", PartnerLocationRow.EmailAddress);
            htmlText += String.Format("<td>{0}</td>", PartnerLocationRow.TelephoneNumber);

            PLocationTable LocationTable = (PLocationTable)ADataSet.Tables[PLocationTable.GetTableName()];
            DataView locationview = LocationTable.DefaultView;
            locationview.Sort = "p_location_key_i ASC, p_site_key_n ASC";
            int indexLocation = locationview.Find(new Object[] {
                    PartnerLocationRow.LocationKey,
                    PartnerLocationRow.SiteKey
                });

            if (indexLocation != -1)
            {
                PLocationRow LocationRow = (PLocationRow)locationview[indexLocation].Row;
                htmlText += String.Format("<td>{0}</td>", LocationRow.StreetName);
                htmlText += String.Format("<td>{0}</td>", LocationRow.Address3);
                htmlText += String.Format("<td>{0}</td>", LocationRow.PostalCode);
                htmlText += String.Format("<td>{0}</td>", LocationRow.City);
                htmlText += String.Format("<td>{0}</td>", LocationRow.CountryCode);
            }
        }

        return htmlText;
    }

    public static void SendEmailToPersonnel(
        DataSet ABirthdayDataSet,
        DataSet AAnniversaryDataSet,
        string AEmailRecipient,
        DateTime AStartDate, DateTime AEndDate,
        string ASpecialBirthdays,
        string ASpecialAnniversaries
        )
    {
        // sort by Birthday this year
        DataView view = new DataView(ABirthdayDataSet.Tables[TDataAnniversaries.BIRTHDAYTABLE]);

        view.Sort = "DOBThisYear ASC";

        string emailText = "<html>  <meta content=\"text/html; charset=UTF-8\" http-equiv=\"content-type\"/><body>" + Environment.NewLine;
        emailText += String.Format("<h3>Birthdays for the date range {0:d-MMM-yyyy} to {1:d-MMM-yyyy}</h3>",
            AStartDate, AEndDate) + Environment.NewLine;
        emailText += String.Format("Round Birthdays: {0}",
            ASpecialBirthdays) + Environment.NewLine;
        emailText += "<table>" + Environment.NewLine;

        foreach (DataRowView birthday in view)
        {
            emailText += "<tr>";
            emailText += String.Format("<td>{0}</td>", birthday["PartnerKey"]);
            emailText += String.Format("<td>{0:d-MMM-yyyy}</td>", birthday["DOB"]);
            emailText += String.Format("<td>{0}</td>", (AEndDate.Year - Convert.ToDateTime(birthday["DOB"]).Year).ToString());
            emailText += String.Format("<td>{0}</td>", birthday["Surname"]);
            emailText += String.Format("<td>{0}</td>", birthday["Firstname"]);

            emailText += AddressToHtml(ABirthdayDataSet, Convert.ToInt64(birthday["PartnerKey"]));

            emailText += "</tr>" + Environment.NewLine;
        }

        emailText += "</table>" + Environment.NewLine;

        view = new DataView(AAnniversaryDataSet.Tables[TDataAnniversaries.ANNIVERSARYTABLE]);
        view.Sort = "AnniversaryDay ASC";
        emailText += String.Format("<h3>Anniversaries for the date range {0:d-MMM-yyyy} to {1:d-MMM-yyyy}</h3>",
            AStartDate, AEndDate) + Environment.NewLine;
        emailText += String.Format("Round Anniversaries: {0}",
            ASpecialAnniversaries) + Environment.NewLine;
        emailText += "<table>" + Environment.NewLine;

        foreach (DataRowView anniversary in view)
        {
            emailText += "<tr>";
            emailText += String.Format("<td>{0}</td>", anniversary["PartnerKey"]);
            emailText += String.Format("<td>{0:d-MMM-yyyy}</td>", anniversary["AnniversaryDay"]);
            emailText += String.Format("<td>{0}</td>", anniversary["TotalYears"]);
            emailText += String.Format("<td>{0}</td>", anniversary["Surname"]);
            emailText += String.Format("<td>{0}</td>", anniversary["Firstname"]);

            emailText += AddressToHtml(AAnniversaryDataSet, Convert.ToInt64(anniversary["PartnerKey"]));

            emailText += "</tr>" + Environment.NewLine;
        }

        emailText += "</table>" + Environment.NewLine;

        emailText += "</body></html>" + Environment.NewLine;

        if (AEmailRecipient.Length != 0)
        {
            TSmtpSender smtp = new TSmtpSender();
            MailMessage newEmail = new MailMessage("petra", AEmailRecipient,
                "Birthdays and Anniversaries", emailText);
            newEmail.IsBodyHtml = true;
            smtp.SendMessage(ref newEmail);
        }
        else
        {
            Console.WriteLine("Subject: birthdays and anniversaries");
            Console.WriteLine("Content-Type: text/html; charset=\"iso-8859-1\"");
            Console.WriteLine();
            Console.WriteLine(emailText);
        }
    }
}
}