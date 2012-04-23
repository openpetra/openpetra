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
using System.Text;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;

namespace Ict.Testing.SampleDataConstructor
{
    /// <summary>
    /// tools for generating partners with FAMILY records
    /// </summary>
    public class SampleDataDonors
    {
        /// <summary>
        /// generate the partners from a text file that was generated with Benerator
        /// </summary>
        /// <param name="AInputBeneratorFile"></param>
        public static void GenerateFamilyPartners(string AInputBeneratorFile)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();

            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AInputBeneratorFile, ",", Encoding.UTF8);

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            while (RecordNode != null)
            {
                string familySituation = TXMLParser.GetAttribute(RecordNode, "familySituation");

                PFamilyRow familyRecord = null;

                if (familySituation == "singleMan")
                {
                    familyRecord = SampleDataWorkers.GenerateFamilyRecord(RecordNode, "Male", MainDS);
                    SampleDataWorkers.GeneratePersonRecord(RecordNode, familyRecord, "Male", MainDS);
                }
                else if (familySituation == "singleWoman")
                {
                    familyRecord = SampleDataWorkers.GenerateFamilyRecord(RecordNode, "Female", MainDS);
                    SampleDataWorkers.GeneratePersonRecord(RecordNode, familyRecord, "Female", MainDS);
                }
                else if (familySituation == "family")
                {
                    familyRecord = SampleDataWorkers.GenerateFamilyRecord(RecordNode, "Male", MainDS);
                    SampleDataWorkers.GeneratePersonRecord(RecordNode, familyRecord, "Male", MainDS);
                    SampleDataWorkers.GeneratePersonRecord(RecordNode, familyRecord, "Female", MainDS);

                    int NumberOfChildren = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "numberOfChildren"));

                    if (NumberOfChildren > 0)
                    {
                        PPartnerRow FamilyPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(familyRecord.PartnerKey);
                        FamilyPartnerRow.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_FAMILY;
                    }
                }

                SampleDataWorkers.GenerateAddressForFamily(RecordNode, familyRecord, MainDS);

                RecordNode = RecordNode.NextSibling;
            }

            // we do not save person records for normal family partners
            MainDS.PPerson.Clear();

            TVerificationResultCollection VerificationResult;
            PartnerEditTDSAccess.SubmitChanges(MainDS, out VerificationResult);

            if (VerificationResult.HasCriticalOrNonCriticalErrors)
            {
                throw new Exception(VerificationResult.BuildVerificationResultString());
            }
        }
    }
}