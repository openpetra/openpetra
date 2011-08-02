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
using System.IO;
using System.Xml;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared.MPartner;

namespace GenerateSampleData
{
/// <summary>
/// Using Data that has been generated with the script from http://www.generatedata.com/#generator,
/// this class provides functions to merge the data and create files that can be imported into OpenPetra
/// </summary>
class TGenerateSampleData
{
    public static void Main(string[] args)
    {
        new TAppSettingsManager(false);

        string csvInputFileName = TAppSettingsManager.GetValue("file", true);

        try
        {
            if (!System.IO.File.Exists(csvInputFileName))
            {
                Console.WriteLine("cannot find file " + csvInputFileName);
                Environment.Exit(-1);
            }

            // load initial csv file.
            // it was generated from generateData (http://www.generatedata.com/#generator),
            // and has a line with the column captions
            // the separator is |
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(csvInputFileName, "|");

            XmlNode partner = doc.DocumentElement.FirstChild;

            while (partner != null)
            {
                TXMLParser.SetAttribute(partner, MPartnerConstants.PARTNERIMPORT_FIRSTNAME, "");

                // TODO: if partner class is PERSON, need to create both FAMILY and PERSON record, if couples this will create several lines?

                // rename the columns
                TXMLParser.RenameAttribute(partner, "Country", MPartnerConstants.PARTNERIMPORT_COUNTRYCODE);
                TXMLParser.RenameAttribute(partner, "Family", MPartnerConstants.PARTNERIMPORT_FAMILYNAME);
                TXMLParser.RenameAttribute(partner, "Address", MPartnerConstants.PARTNERIMPORT_STREETNAME);
                TXMLParser.RenameAttribute(partner, "PostCode", MPartnerConstants.PARTNERIMPORT_POSTALCODE);

                // calculate first name, using the gender. use male first name or female first name, etc
                if (partner.Attributes["Gender"].Value == "Male")
                {
                    partner.Attributes[MPartnerConstants.PARTNERIMPORT_FIRSTNAME].Value = partner.Attributes["FirstnameMale"].Value;
                    TXMLParser.SetAttribute(partner, MPartnerConstants.PARTNERIMPORT_TITLE, "Mr");
                }
                else if (partner.Attributes["Gender"].Value == "Female")
                {
                    partner.Attributes[MPartnerConstants.PARTNERIMPORT_FIRSTNAME].Value = partner.Attributes["FirstnameFemale"].Value;
                    TXMLParser.SetAttribute(partner, MPartnerConstants.PARTNERIMPORT_TITLE, "Mrs");
                }
                else
                {
                    partner.Attributes[MPartnerConstants.PARTNERIMPORT_FIRSTNAME].Value =
                        partner.Attributes["FirstnameMale"].Value + " and " +
                        partner.Attributes["FirstnameFemale"].Value;
                    TXMLParser.SetAttribute(partner, MPartnerConstants.PARTNERIMPORT_TITLE, "Mr and Mrs");
                }

                // drop unwanted columns
                partner.Attributes.Remove(partner.Attributes["FirstnameMale"]);
                partner.Attributes.Remove(partner.Attributes["FirstnameFemale"]);
                partner.Attributes.Remove(partner.Attributes["Child1"]);
                partner.Attributes.Remove(partner.Attributes["DonationDate1"]);
                partner.Attributes.Remove(partner.Attributes["DonationRecipient1"]);
                partner.Attributes.Remove(partner.Attributes["DonationAmount1"]);
                partner.Attributes.Remove(partner.Attributes["PartnerClass"]);
                partner = partner.NextSibling;
            }

            // TODO generate separate donation import files???

            // store csv file with correct column captions, and use local default CSV separator
            string csvOutputFileName = Path.GetDirectoryName(csvInputFileName) +
                                       Path.DirectorySeparatorChar +
                                       Path.GetFileNameWithoutExtension(csvInputFileName) + "2.csv";
            TCsv2Xml.Xml2Csv(doc, csvOutputFileName);
            Console.WriteLine(csvOutputFileName + " was written.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}
}