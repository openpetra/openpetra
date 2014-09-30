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
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Tools.SampleDataConstructor
{
    /// <summary>
    /// tools for generating people with FAMILY and PERSON partner records, and commitment data
    /// </summary>
    public class SampleDataWorkers
    {
        /// <summary>
        /// generate the partners from a text file that was generated with Benerator
        /// </summary>
        /// <param name="AInputBeneratorFile"></param>
        public static void GenerateWorkers(string AInputBeneratorFile)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            PersonnelTDS PersonnelDS = new PersonnelTDS();

            // get a list of fields (all class UNIT, with unit type F)
            string sqlGetFieldPartnerKeys = "SELECT p_partner_key_n, p_unit_name_c FROM PUB_p_unit WHERE u_unit_type_code_c = 'F'";
            DataTable FieldKeys = DBAccess.GDBAccessObj.SelectDT(sqlGetFieldPartnerKeys, "keys", null);

            // get a list of banks (all class BANK)
            string sqlGetBankPartnerKeys = "SELECT p_partner_key_n FROM PUB_p_bank";
            DataTable BankKeys = DBAccess.GDBAccessObj.SelectDT(sqlGetBankPartnerKeys, "keys", null);

            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AInputBeneratorFile, ",", Encoding.UTF8);

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            while (RecordNode != null)
            {
                string familySituation = TXMLParser.GetAttribute(RecordNode, "familySituation");

                PFamilyRow familyRecord = null;

                if (familySituation == "singleMan")
                {
                    familyRecord = GenerateFamilyRecord(RecordNode, "Male", MainDS);
                    GeneratePersonRecord(RecordNode, familyRecord, "Male", MainDS);
                }
                else if (familySituation == "singleWoman")
                {
                    familyRecord = GenerateFamilyRecord(RecordNode, "Female", MainDS);
                    GeneratePersonRecord(RecordNode, familyRecord, "Female", MainDS);
                }
                else if (familySituation == "family")
                {
                    familyRecord = GenerateFamilyRecord(RecordNode, "Male", MainDS);
                    GeneratePersonRecord(RecordNode, familyRecord, "Male", MainDS);
                    GeneratePersonRecord(RecordNode, familyRecord, "Female", MainDS);

                    int AgeDifferenceSpouse = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "AgeDifferenceSpouse"));
                    DataView FamilyView = new DataView(MainDS.PPerson);
                    FamilyView.RowFilter = PPersonTable.GetFamilyKeyDBName() + " = " + familyRecord.PartnerKey.ToString();
                    FamilyView.Sort = PPersonTable.GetFamilyIdDBName();

                    PPersonRow HusbandPersonRow = (PPersonRow)FamilyView[0].Row;
                    PPersonRow WifePersonRow = (PPersonRow)FamilyView[1].Row;
                    WifePersonRow.DateOfBirth =
                        WifePersonRow.DateOfBirth.Value.AddYears(
                            AgeDifferenceSpouse - (WifePersonRow.DateOfBirth.Value.Year - HusbandPersonRow.DateOfBirth.Value.Year));

                    if (DateTime.Today.Year - WifePersonRow.DateOfBirth.Value.Year < 19)
                    {
                        WifePersonRow.DateOfBirth.Value.AddYears(
                            19 - (DateTime.Today.Year - WifePersonRow.DateOfBirth.Value.Year));
                    }

                    int NumberOfChildren = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "numberOfChildren"));

                    for (int countChild = 0; countChild < NumberOfChildren; countChild++)
                    {
                        DateTime DateOfBirthChild = Convert.ToDateTime(
                            TXMLParser.GetAttribute(RecordNode,
                                "Child" + (countChild + 1).ToString() + "DateOfBirth"));

                        // mother must have been 19 when the child was born
                        if (DateOfBirthChild.Year < WifePersonRow.DateOfBirth.Value.Year + 19)
                        {
                            continue;
                        }

                        GeneratePersonRecord(RecordNode, familyRecord, "Child" + (countChild + 1).ToString(), MainDS);
                    }
                }

                GenerateAddressForFamily(RecordNode, familyRecord, MainDS);

                GenerateCommitmentRecord(RecordNode, familyRecord, MainDS, PersonnelDS, FieldKeys);

                GenerateBankDetails(RecordNode, familyRecord, MainDS, BankKeys);

                if (MainDS.PFamily.Rows.Count % 100 == 0)
                {
                    TLogging.Log("created worker " + MainDS.PFamily.Rows.Count.ToString() + " " + familyRecord.FamilyName);
                }

                RecordNode = RecordNode.NextSibling;
            }

            MainDS.ThrowAwayAfterSubmitChanges = true;

            PartnerEditTDSAccess.SubmitChanges(MainDS);

            PersonnelDS.ThrowAwayAfterSubmitChanges = true;
            PersonnelTDSAccess.SubmitChanges(PersonnelDS);

            TLogging.Log("after saving workers");
        }

        static Int32 NumberOfPartnerKeysReserved = 0;
        static Int64 NextPartnerKey = -1;

        /// <summary>
        /// generate a family record
        /// </summary>
        public static PFamilyRow GenerateFamilyRecord(XmlNode ACurrentNode, string APrefix, PartnerEditTDS AMainDS)
        {
            PFamilyRow FamilyRow = AMainDS.PFamily.NewRowTyped();
            PPartnerRow PartnerRow = AMainDS.PPartner.NewRowTyped();

            if (NumberOfPartnerKeysReserved == 0)
            {
                NumberOfPartnerKeysReserved = 100;
                NextPartnerKey = TNewPartnerKey.ReservePartnerKeys(-1, ref NumberOfPartnerKeysReserved);
            }

            long PartnerKey = NextPartnerKey;
            NextPartnerKey++;
            NumberOfPartnerKeysReserved--;

            PartnerRow.PartnerKey = PartnerKey;
            FamilyRow.PartnerKey = PartnerRow.PartnerKey;

            FamilyRow.FirstName = TXMLParser.GetAttribute(ACurrentNode, APrefix + "FirstName");
            FamilyRow.FamilyName = TXMLParser.GetAttribute(ACurrentNode, APrefix + "FamilyName");
            FamilyRow.Title = TXMLParser.GetAttribute(ACurrentNode, APrefix + "Title");

            PartnerRow.PartnerShortName = Calculations.DeterminePartnerShortName(
                FamilyRow.FamilyName,
                FamilyRow.Title,
                FamilyRow.FirstName);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            AMainDS.PFamily.Rows.Add(FamilyRow);
            AMainDS.PPartner.Rows.Add(PartnerRow);

            return FamilyRow;
        }

        /// <summary>
        /// generate a person record and update the associated family partner
        /// </summary>
        /// <param name="ACurrentNode"></param>
        /// <param name="AFamilyRow"></param>
        /// <param name="APrefix"></param>
        /// <param name="AMainDS"></param>
        public static void GeneratePersonRecord(XmlNode ACurrentNode, PFamilyRow AFamilyRow, string APrefix, PartnerEditTDS AMainDS)
        {
            PPersonRow PersonRow = AMainDS.PPerson.NewRowTyped();
            PPartnerRow PartnerRow = AMainDS.PPartner.NewRowTyped();

            if (NumberOfPartnerKeysReserved == 0)
            {
                NumberOfPartnerKeysReserved = 100;
                NextPartnerKey = TNewPartnerKey.ReservePartnerKeys(-1, ref NumberOfPartnerKeysReserved);
            }

            long PartnerKey = NextPartnerKey;
            NextPartnerKey++;
            NumberOfPartnerKeysReserved--;

            PartnerRow.PartnerKey = PartnerKey;
            PersonRow.PartnerKey = PartnerRow.PartnerKey;
            PersonRow.FamilyKey = AFamilyRow.PartnerKey;

            // create family id: count existing family members
            DataView FamilyView = new DataView(AMainDS.PPerson);
            FamilyView.RowFilter = PPersonTable.GetFamilyKeyDBName() + " = " + AFamilyRow.PartnerKey.ToString();
            FamilyView.Sort = PPersonTable.GetFamilyIdDBName();

            PersonRow.FamilyId = FamilyView.Count;

            PersonRow.FirstName = TXMLParser.GetAttribute(ACurrentNode, APrefix + "FirstName");
            PersonRow.MiddleName1 = TXMLParser.GetAttribute(ACurrentNode, APrefix + "MiddleName");
            PersonRow.FamilyName = AFamilyRow.FamilyName;

            if (FamilyView.Count == 1)
            {
                PartnerRow.PreviousName = TXMLParser.GetAttribute(ACurrentNode, APrefix + "FamilyName");
            }

            PersonRow.Title = TXMLParser.GetAttribute(ACurrentNode, APrefix + "Title");

            PartnerRow.PartnerShortName = Calculations.DeterminePartnerShortName(
                PersonRow.FamilyName,
                PersonRow.Title,
                PersonRow.FirstName);
            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
            PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            PersonRow.DateOfBirth = Convert.ToDateTime(TXMLParser.GetAttribute(ACurrentNode, APrefix + "DateOfBirth"));

            string gender = TXMLParser.GetAttribute(ACurrentNode, APrefix + "Gender");

            if ((APrefix == "Male") || (gender == "MALE"))
            {
                PersonRow.Gender = MPartnerConstants.GENDER_MALE;
                PartnerRow.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_MALE;
            }
            else if ((APrefix == "Female") || (gender == "FEMALE"))
            {
                PersonRow.Gender = MPartnerConstants.GENDER_FEMALE;
                PartnerRow.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_FEMALE;
            }

            PPartnerRow FamilyPartnerRow = (PPartnerRow)AMainDS.PPartner.Rows.Find(AFamilyRow.PartnerKey);

            if (FamilyView.Count == 0)
            {
                FamilyPartnerRow.AddresseeTypeCode = PartnerRow.AddresseeTypeCode;
            }
            else if (FamilyView.Count == 1)
            {
                // this is a couple
                PersonRow.MaritalStatus = MPartnerConstants.MARITALSTATUS_MARRIED;

                // find first person record as well
                PPersonRow Husband = (PPersonRow)FamilyView[0].Row;
                Husband.MaritalStatus = MPartnerConstants.MARITALSTATUS_MARRIED;

                AFamilyRow.MaritalStatus = MPartnerConstants.MARITALSTATUS_MARRIED;

                AFamilyRow.FirstName = Husband.FirstName + " and " + PersonRow.FirstName;
                AFamilyRow.Title = Husband.Title + " and " + PersonRow.Title;

                // update family shortname
                FamilyPartnerRow.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_COUPLE;
                FamilyPartnerRow.PartnerShortName = Calculations.DeterminePartnerShortName(
                    AFamilyRow.FamilyName,
                    AFamilyRow.Title,
                    AFamilyRow.FirstName);
            }
            else if (FamilyView.Count > 1)
            {
                FamilyPartnerRow.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_FAMILY;
            }

            AMainDS.PPerson.Rows.Add(PersonRow);
            AMainDS.PPartner.Rows.Add(PartnerRow);
        }

        /// <summary>
        /// create PPartnerLocation records for the FAMILY partner, and all the PERSON records of this family
        /// </summary>
        public static void GenerateAddressForFamily(XmlNode ACurrentNode, PFamilyRow AFamilyRow, PartnerEditTDS AMainDS)
        {
            PLocationRow locationRow = AMainDS.PLocation.NewRowTyped();

            locationRow.SiteKey = 0; // DomainManager.GSiteKey;
            locationRow.LocationKey = (AMainDS.PLocation.Count + 1) * -1;
            locationRow.StreetName = TXMLParser.GetAttribute(ACurrentNode, "Addr2");
            locationRow.PostalCode = TXMLParser.GetAttribute(ACurrentNode, "PostCode");
            locationRow.City = TXMLParser.GetAttribute(ACurrentNode, "City");
            locationRow.County = TXMLParser.GetAttribute(ACurrentNode, "Province");
            locationRow.CountryCode = TXMLParser.GetAttribute(ACurrentNode, "CountryCode");

            AMainDS.PLocation.Rows.Add(locationRow);

            PPartnerLocationRow familyLocationRow = AMainDS.PPartnerLocation.NewRowTyped();

            familyLocationRow.PartnerKey = AFamilyRow.PartnerKey;
            familyLocationRow.LocationKey = locationRow.LocationKey;
            familyLocationRow.SiteKey = locationRow.SiteKey;
            familyLocationRow.LocationType = MPartnerConstants.LOCATIONTYPE_HOME;
            familyLocationRow.SendMail = true;

            AMainDS.PPartnerLocation.Rows.Add(familyLocationRow);

            DataView FamilyView = new DataView(AMainDS.PPerson);
            FamilyView.RowFilter = PPersonTable.GetFamilyKeyDBName() + " = " + AFamilyRow.PartnerKey.ToString();
            FamilyView.Sort = PPersonTable.GetFamilyIdDBName();

            // for each person, also create a location record
            for (int countPerson = 0; countPerson < FamilyView.Count; countPerson++)
            {
                PPersonRow personRow = (PPersonRow)FamilyView[countPerson].Row;

                PPartnerLocationRow personLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
                personLocationRow.PartnerKey = personRow.PartnerKey;
                personLocationRow.LocationKey = familyLocationRow.LocationKey;
                personLocationRow.SiteKey = familyLocationRow.SiteKey;
                personLocationRow.LocationType = MPartnerConstants.LOCATIONTYPE_HOME;
                personLocationRow.SendMail = true;
                AMainDS.PPartnerLocation.Rows.Add(personLocationRow);

                string prefix = "Male";

                if (countPerson == 1)
                {
                    prefix = "Female";
                }
                else if (countPerson > 1)
                {
                    prefix = "Child" + (countPerson - 1).ToString();
                }

                personLocationRow.EmailAddress = TXMLParser.GetAttribute(ACurrentNode, prefix + "Email");

                // set email of first person for whole family
                if (countPerson == 0)
                {
                    familyLocationRow.EmailAddress = personLocationRow.EmailAddress;
                }
            }
        }

        private static void GenerateCommitmentRecord(
            XmlNode ACurrentNode,
            PFamilyRow AFamilyRow,
            PartnerEditTDS AMainDS,
            PersonnelTDS APersonnelDS,
            DataTable AFieldKeys)
        {
            DataView FamilyView = new DataView(AMainDS.PPerson);

            FamilyView.RowFilter = PPersonTable.GetFamilyKeyDBName() + " = " + AFamilyRow.PartnerKey.ToString();
            FamilyView.Sort = PPersonTable.GetFamilyIdDBName();
            PPersonRow firstPerson = (PPersonRow)FamilyView[0].Row;

            int FieldID =
                Convert.ToInt32(TXMLParser.GetAttribute(ACurrentNode, "fieldCommitment")) % AFieldKeys.Rows.Count;
            long FieldPartnerKey = Convert.ToInt64(AFieldKeys.Rows[FieldID].ItemArray[0]);

            PPartnerGiftDestinationRow giftDestination = AMainDS.PPartnerGiftDestination.NewRowTyped();
            giftDestination.Key = TPartnerDataReaderWebConnector.GetNewKeyForPartnerGiftDestination();
            giftDestination.FieldKey = FieldPartnerKey;
            giftDestination.DateEffective = Convert.ToDateTime(TXMLParser.GetAttribute(ACurrentNode, "startDateCommitment"));

            PmStaffDataRow staffData = APersonnelDS.PmStaffData.NewRowTyped();
            staffData.SiteKey = DomainManager.GSiteKey;
            staffData.Key = (APersonnelDS.PmStaffData.Count + 1) * -1;
            staffData.PartnerKey = firstPerson.PartnerKey;
            staffData.ReceivingField = FieldPartnerKey;

            // TODO: could add foreign nationals
            staffData.HomeOffice = DomainManager.GSiteKey;
            staffData.OfficeRecruitedBy = DomainManager.GSiteKey;

            staffData.StartOfCommitment = Convert.ToDateTime(TXMLParser.GetAttribute(ACurrentNode, "startDateCommitment"));
            int LengthCommitment = Convert.ToInt32(TXMLParser.GetAttribute(ACurrentNode, "lengthCommitment"));

            staffData.StatusCode = "LONG-TERMER";

            if (LengthCommitment > 0)
            {
                string LengthCommitmentUnit = TXMLParser.GetAttribute(ACurrentNode, "lengthCommitmentUnit");

                if (LengthCommitmentUnit == "week")
                {
                    staffData.StatusCode = "SHORT-TERMER";
                    staffData.EndOfCommitment = staffData.StartOfCommitment.AddDays(7 * LengthCommitment);
                }
                else if (LengthCommitmentUnit == "month")
                {
                    staffData.StatusCode = "SHORT-TERMER";
                    staffData.EndOfCommitment = staffData.StartOfCommitment.AddMonths(LengthCommitment);
                }
                else if (LengthCommitmentUnit == "year")
                {
                    if (LengthCommitment < 3)
                    {
                        staffData.StatusCode = "WORKER";
                    }

                    staffData.EndOfCommitment = staffData.StartOfCommitment.AddYears(LengthCommitment);
                }
            }

            APersonnelDS.PmStaffData.Rows.Add(staffData);

            giftDestination.DateExpires = staffData.EndOfCommitment;

            if (AMainDS.PPartnerGiftDestination == null)
            {
                AMainDS.PPartnerGiftDestination.Merge(new PPartnerGiftDestinationTable());
            }

            AMainDS.PPartnerGiftDestination.Rows.Add(giftDestination);

            // TODO depending on start and end date of commitment, set EX-WORKER or no special type yet at all
            string SpecialType = MPartnerConstants.PARTNERTYPE_WORKER;

            if (!staffData.IsEndOfCommitmentNull() && (staffData.EndOfCommitment < DateTime.Today))
            {
                SpecialType = MPartnerConstants.PARTNERTYPE_EX_WORKER;
            }

            if (SpecialType != string.Empty)
            {
                // create special type for family partner
                PPartnerTypeRow PartnerTypeRow = AMainDS.PPartnerType.NewRowTyped();
                PartnerTypeRow.PartnerKey = AFamilyRow.PartnerKey;
                PartnerTypeRow.TypeCode = SpecialType;
                AMainDS.PPartnerType.Rows.Add(PartnerTypeRow);

                // set special type WORKER for the parents
                for (int countPerson = 0; countPerson < FamilyView.Count && countPerson < 2; countPerson++)
                {
                    PPersonRow personRow = (PPersonRow)FamilyView[countPerson].Row;

                    // create special type for the person partners
                    PartnerTypeRow = AMainDS.PPartnerType.NewRowTyped();
                    PartnerTypeRow.PartnerKey = personRow.PartnerKey;
                    PartnerTypeRow.TypeCode = SpecialType;
                    AMainDS.PPartnerType.Rows.Add(PartnerTypeRow);
                }
            }
        }

        /// <summary>
        /// create PPartnerBankingDetail records for the FAMILY partner
        /// </summary>
        public static void GenerateBankDetails(XmlNode ACurrentNode, PFamilyRow AFamilyRow, PartnerEditTDS AMainDS,
            DataTable ABankKeys)
        {
            if (TXMLParser.HasAttribute(ACurrentNode, "bankaccount_bank"))
            {
                Int32 BankID =
                    Convert.ToInt32(TXMLParser.GetAttribute(ACurrentNode, "bankaccount_bank")) % ABankKeys.Rows.Count;
                long BankPartnerKey = Convert.ToInt64(ABankKeys.Rows[BankID].ItemArray[0]);

                PBankingDetailsRow bankingDetailsRow = AMainDS.PBankingDetails.NewRowTyped();

                bankingDetailsRow.BankingDetailsKey = (AMainDS.PBankingDetails.Rows.Count + 1) * -1;
                bankingDetailsRow.BankKey = BankPartnerKey;
                bankingDetailsRow.BankAccountNumber = TXMLParser.GetAttribute(ACurrentNode, "bankaccount_account");
                bankingDetailsRow.BankingType = MPartnerConstants.BANKINGTYPE_BANKACCOUNT;
                bankingDetailsRow.AccountName = AFamilyRow.FirstName + " " + AFamilyRow.FamilyName;

                AMainDS.PBankingDetails.Rows.Add(bankingDetailsRow);

                PPartnerBankingDetailsRow partnerBankingRow = AMainDS.PPartnerBankingDetails.NewRowTyped();
                partnerBankingRow.PartnerKey = AFamilyRow.PartnerKey;
                partnerBankingRow.BankingDetailsKey = bankingDetailsRow.BankingDetailsKey;
                AMainDS.PPartnerBankingDetails.Rows.Add(partnerBankingRow);

                PBankingDetailsUsageRow bankingUsageRow = AMainDS.PBankingDetailsUsage.NewRowTyped();
                bankingUsageRow.BankingDetailsKey = bankingDetailsRow.BankingDetailsKey;
                bankingUsageRow.PartnerKey = AFamilyRow.PartnerKey;
                bankingUsageRow.Type = MPartnerConstants.BANKINGUSAGETYPE_MAIN;
                AMainDS.PBankingDetailsUsage.Rows.Add(bankingUsageRow);
            }
        }
    }
}