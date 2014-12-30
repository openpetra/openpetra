//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2014 by OM International
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

// This code is based on the "Original Petra" 4GL file CalebExport.i

using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Common;
using System.Net.Mail;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;

namespace Ict.Petra.Server.MSysMan.ImportExport.WebConnectors
{
    /// <summary>
    ///
    /// </summary>
    public class TIntranetExportWebConnector
    {
        private const string ExportDateFormat = "yyyy/MM/dd";
        private const string ExportVersion = "1.1.7";        // 1.1.7 is Petra's version - perhaps I should use a different one?
        private static TDBTransaction FTransaction;
        private static string FExportTrace;
        private static String FExportFilePath;
        private static List <String>FZipFileNames = new List <String>();

        private class PartnerDetails
        {
            public String FirstName;
            public String LastName;
            public String Email;
            public String Class;
            public Boolean Anonymous;
            public string Telephone;
            public String Address;
        }

        private static SortedList <Int64, PartnerDetails>DonorList = new SortedList <Int64, PartnerDetails>();
        private static SortedList <Int64, PartnerDetails>RecipientList = new SortedList <Int64, PartnerDetails>();

        private class PersonRec
        {
            public String Title;                //  LIKE p_person.p_title_c
            public String FirstName;            //  LIKE p_person.p_first_name_c
            public String MiddleName;           //  LIKE p_person.p_middle_name_1_c
            public String LastName;             //  LIKE p_person.p_family_name_c
            public String Academic;             //  LIKE p_person.p_academic_title_c
            public String Decorations;          //  LIKE p_person.p_decorations_c
            public String Gender;               //  LIKE p_person.p_gender_c
            public String PreferredName;        //  LIKE p_person.p_prefered_name_c
            public Int64 FamilyKey;             //  LIKE p_person.p_family_key_n
            public Int64 HomeOffice;            //  LIKE pm_staff_data.pm_home_office_n
            public DateTime DateOfBirth;        //  LIKE p_person.p_date_of_birth_d
            public String EmailAddress;         //  LIKE p_partner_location.p_email_address_c
            public String LocationType;         //  LIKE p_partner_location.p_location_type_c
        };


        private class BatchKey
        {
            public Int32 LedgerNumber;
            public Int32 BatchNumber;
        }

        private static List <BatchKey>GiftBatches = new List <BatchKey>();

        /// <summary>
        /// Delegate for the determination of the 'Primary Phone Number' and the 'Primary E-mail Address' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary E-mail Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got at least one of the 'Primary E-mail Address' and the 'Primary Phone Number'
        /// Contact Details, otherwise false.</returns>
        [NoRemoting]
        public delegate bool GetPrimaryEmailAndPrimaryPhone(Int64 APartnerKey,
            out string APrimaryPhoneNumber, out string APrimaryEmailAddress);

        /// <summary>
        /// Delegate for the determination of the 'Within Organisation E-mail Address' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="AWithinOrganisationEmailAddress">The 'Within Organisation Email Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Within Organisation Email Address', otherwise false.</returns>
        [NoRemoting]
        public delegate bool GetWithinOrganisationEmail(Int64 APartnerKey,
            out string AWithinOrganisationEmailAddress);

        /// <summary>
        ///
        /// </summary>
        [NoRemoting]
        public static GetPrimaryEmailAndPrimaryPhone GetPrimaryEmailAndPrimaryPhoneDelegate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [NoRemoting]
        public static GetWithinOrganisationEmail GetWithinOrganisationEmailDelegate
        {
            get;
            set;
        }

/*
 *      // I need to call MPartner.ServerCalculations.DetermineBestAddress through a delegate,
 *      // since I can't refer to it directly from here. The delegate will have been set up
 *      // to point to the correct function by a previous call to TCallForwarding().
 *
 *      /// <summary>
 *      ///
 *      /// </summary>
 *      /// <param name="APartnerKey"></param>
 *      /// <param name="LocationRow"></param>
 *      /// <returns></returns>
 *      [NoRemoting]
 *      public delegate TLocationPK GetLocationRow(Int64 APartnerKey, out PPartnerLocationRow LocationRow);
 *      private static GetLocationRow FGetLocationRowDelegate;
 *
 *      /// <summary>
 *      ///
 *      /// </summary>
 *      [NoRemoting]
 *      public static GetLocationRow GetLocationRowDelegate
 *      {
 *          get
 *          {
 *              return FGetLocationRowDelegate;
 *          }
 *          set
 *          {
 *              FGetLocationRowDelegate = value;
 *          }
 *      }
 */
        private static String PutDate(object DateField)
        {
            String ret = "";

            if (DateField.GetType() != typeof(System.DBNull))
            {
                DateTime DateOrNull = (DateTime)DateField;

                if (DateOrNull.Year > 1) // When a DateTime is not initialised, it's 01/01/0001
                {
                    ret = DateOrNull.ToString(ExportDateFormat);
                }
            }

            return ret;
        }

        private static PartnerDetails GetDonor(Int64 APartnerKey)
        {
            if (DonorList.ContainsKey(APartnerKey))
            {
                return DonorList[APartnerKey];
            }

            PartnerDetails Ret = new PartnerDetails();
            PPartnerTable PartnerTbl = PPartnerAccess.LoadByPrimaryKey(APartnerKey, FTransaction);

            if (PartnerTbl.Rows.Count > 0)
            {
                PPartnerRow PartnerRow = PartnerTbl[0];

                Ret.LastName = PartnerRow.PartnerShortName;
                Ret.Anonymous = PartnerRow.AnonymousDonor;

                if (PartnerRow.PartnerClass == "PERSON")
                {
                    PPersonTable PersonTbl = PPersonAccess.LoadByPrimaryKey(APartnerKey, FTransaction);

                    if (PersonTbl.Rows.Count > 0)
                    {
                        PPersonRow PersonRow = PersonTbl[0];
                        Ret.FirstName = PersonRow.FirstName;
                        Ret.LastName = PersonRow.FamilyName;
                        Ret.Class = "PERSON";
                    }
                }

                if (PartnerRow.PartnerClass == "FAMILY")
                {
                    PFamilyTable FamilyTbl = PFamilyAccess.LoadByPrimaryKey(APartnerKey, FTransaction);

                    if (FamilyTbl.Rows.Count > 0)
                    {
                        PFamilyRow FamilyRow = FamilyTbl[0];
                        Ret.FirstName = FamilyRow.FirstName;
                        Ret.LastName = FamilyRow.FamilyName;
                        Ret.Class = "FAMILY";
                    }
                }

                PPartnerLocationRow PartnerLocationRow;
                TLocationPK LocationKey = ServerCalculations.DetermineBestAddress(APartnerKey, out PartnerLocationRow);

                if (LocationKey.LocationKey != -1)
                {
                    PLocationTable LocationTbl = PLocationAccess.LoadByPrimaryKey(PartnerLocationRow.SiteKey,
                        PartnerLocationRow.LocationKey,
                        FTransaction);

                    if (LocationTbl.Rows.Count > 0)
                    {
                        PLocationRow LocationRow = LocationTbl[0];
                        Ret.Address = Calculations.DetermineLocationString(LocationRow, Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
                    }
                }

                GetPrimaryEmailAndPrimaryPhoneDelegate(APartnerKey,
                    out Ret.Telephone, out Ret.Email);
            }

            DonorList.Add(APartnerKey, Ret);
            return Ret;
        }

        private static PartnerDetails GetRecipient(Int64 APartnerKey)
        {
            if (RecipientList.ContainsKey(APartnerKey))
            {
                return RecipientList[APartnerKey];
            }

            PartnerDetails Ret = new PartnerDetails();
            PPartnerTable PartnerTbl = PPartnerAccess.LoadByPrimaryKey(APartnerKey, FTransaction);

            if (PartnerTbl.Rows.Count > 0)
            {
                PPartnerRow PartnerRow = PartnerTbl[0];

                Ret.LastName = PartnerRow.PartnerShortName;

                if (PartnerRow.PartnerClass == "PERSON")
                {
                    PPersonTable PersonTbl = PPersonAccess.LoadByPrimaryKey(APartnerKey, FTransaction);

                    if (PersonTbl.Rows.Count > 0)
                    {
                        PPersonRow PersonRow = PersonTbl[0];
                        Ret.FirstName = PersonRow.FirstName;
                        Ret.LastName = PersonRow.FamilyName;
                        Ret.Class = "PERSON";
                    }
                }

                if (PartnerRow.PartnerClass == "FAMILY")
                {
                    PFamilyTable FamilyTbl = PFamilyAccess.LoadByPrimaryKey(APartnerKey, FTransaction);

                    if (FamilyTbl.Rows.Count > 0)
                    {
                        PFamilyRow FamilyRow = FamilyTbl[0];
                        Ret.FirstName = FamilyRow.FirstName;
                        Ret.LastName = FamilyRow.FamilyName;
                        Ret.Class = "FAMILY";
                    }
                }

                GetWithinOrganisationEmailDelegate(APartnerKey, out Ret.Email);

                // TODO Contact Details: Decide what to do if no 'Within Organisation Email Address' is set: nothing, or take the 'Primary E-Mail Address'?
            }

            RecipientList.Add(APartnerKey, Ret);
            return Ret;
        }

        private static void GetGiftBatches(Int32 ADaySpan)
        {
            DateTime GiftsSince = DateTime.Now.AddDays(0 - ADaySpan);

            String SqlQuery = "SELECT " +
                              "a_batch_number_i AS BatchNumber, " +
                              "a_ledger_number_i AS LedgerNumber " +
                              "FROM PUB_a_batch " +
                              "WHERE a_batch_description_c LIKE 'Gift Batch %' " +
                              "AND a_date_of_entry_d > '" + GiftsSince.ToString("yyyy-MM-dd") + "' "
            ;
            DataSet GiftBatchDS = DBAccess.GDBAccessObj.Select(SqlQuery, "GiftBatchTbl", FTransaction);

            GiftBatches.Clear();

            foreach (DataRow Row in GiftBatchDS.Tables["GiftBatchTbl"].Rows)
            {
                BatchKey NewKey = new BatchKey();
                NewKey.BatchNumber = Convert.ToInt32(Row["BatchNumber"]);
                NewKey.LedgerNumber = Convert.ToInt32(Row["LedgerNumber"]);
                GiftBatches.Add(NewKey);
            }
        }

        private static void GetAdminFees(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 ATransactionNumber,
            Int32 ADetailNumber,
            out decimal GIFFee,
            out decimal ICTFee,
            out decimal OtherFee)
        {
            GIFFee = 0;
            ICTFee = 0;
            OtherFee = 0;

            AProcessedFeeTable ProcessedFeeTbl = AProcessedFeeAccess.LoadViaAGiftDetail(ALedgerNumber,
                ABatchNumber,
                ATransactionNumber,
                ADetailNumber,
                FTransaction);

            foreach (AProcessedFeeRow Row in ProcessedFeeTbl.Rows)
            {
                switch (Row.FeeCode)
                {
                    case "ICT":
                    {
                        ICTFee += Row.PeriodicAmount;
                        break;
                    }

                    case "GIF":
                    {
                        GIFFee += Row.PeriodicAmount;
                        break;
                    }

                    default:
                    {
                        OtherFee += Row.PeriodicAmount;
                        break;
                    }
                }
            }
        }

        private static Boolean ExportDonations(Int32 ADaySpan)
        {
            GetGiftBatches(ADaySpan);

            StreamWriter sw1 = File.CreateText(FExportFilePath + "donor.csv");
            sw1.WriteLine("first_name,last_name,partner_key,email,address,telephone,anonymous,class");
            StreamWriter sw2 = File.CreateText(FExportFilePath + "donation.csv");
            sw2.WriteLine("donor,recipient,trans_amount,trans_currency,base_amount,admin_gif,admin_ict,admin_other," +
                "base_currency,intl_amount,intl_currency,date,id,source_ledger,recipient_field,anonymous,comment1,for1,comment2,for2,comment3,for3");

            foreach (BatchKey Batch in GiftBatches)
            {
                String GiftBatchQuery = "SELECT " +
                                        "PUB_a_gift.p_donor_key_n AS DonorKey, " +
                                        "PUB_a_gift_detail.p_recipient_key_n AS RecipientKey, " +
                                        "PUB_a_gift_detail.a_gift_transaction_amount_n AS TransactionAmount, " +
                                        "PUB_a_gift_batch.a_currency_code_c AS CurrencyCode, " +
                                        "PUB_a_gift_detail.a_gift_amount_n AS GiftAmount, " +
                                        "PUB_a_ledger.a_base_currency_c AS BaseCurrency, " +
                                        "PUB_a_gift_detail.a_gift_amount_intl_n AS IntlAmount, " +
                                        "PUB_a_gift_detail.a_cost_centre_code_c AS CostCentre, " +
                                        "PUB_a_ledger.a_intl_currency_c AS IntlCurrency, " +
                                        "PUB_a_gift_batch.a_gl_effective_date_d AS EffectiveDate, " +
                                        "PUB_a_gift_batch.a_batch_number_i AS BatchNumber, " +
                                        "PUB_a_gift.a_gift_transaction_number_i AS TransactionNumber, " +
                                        "PUB_a_gift_detail.a_detail_number_i AS DetailNumber, " +
                                        "PUB_a_gift_detail.a_confidential_gift_flag_l AS Confidential, " +
                                        "PUB_a_gift_detail.a_gift_comment_one_c AS CommentOne, " +
                                        "PUB_a_gift_detail.a_comment_one_type_c AS CommentOneType, " +
                                        "PUB_a_gift_detail.a_gift_comment_two_c AS CommentTwo, " +
                                        "PUB_a_gift_detail.a_comment_two_type_c AS CommentTwoType, " +
                                        "PUB_a_gift_detail.a_gift_comment_three_c AS CommentThree, " +
                                        "PUB_a_gift_detail.a_comment_three_type_c AS CommentThreeType " +
                                        "FROM PUB_a_gift_batch " +
                                        "LEFT JOIN PUB_a_gift ON PUB_a_gift_batch.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_a_gift_batch.a_ledger_number_i = PUB_a_gift.a_ledger_number_i "
                                        +
                                        "LEFT JOIN PUB_a_gift_detail on PUB_a_gift_detail.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_a_gift_detail.a_gift_transaction_number_i  = PUB_a_gift.a_gift_transaction_number_i "
                                        +
                                        "LEFT JOIN PUB_a_motivation_detail ON PUB_a_motivation_detail.a_ledger_number_i = PUB_a_gift_detail.a_ledger_number_i AND PUB_a_motivation_detail.a_motivation_group_code_c = PUB_a_gift_detail.a_motivation_group_code_c AND PUB_a_motivation_detail.a_motivation_detail_code_c = PUB_a_gift_detail.a_motivation_detail_code_c "
                                        +
                                        "LEFT JOIN PUB_p_partner on PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n " +
                                        "LEFT JOIN PUB_a_ledger on PUB_a_gift_batch.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i " +
                                        "WHERE PUB_a_gift_batch.a_ledger_number_i = " + Batch.LedgerNumber + " " +
                                        "AND PUB_a_gift_batch.a_batch_number_i = " + Batch.BatchNumber + " " +
                                        "AND PUB_a_motivation_detail.a_export_to_intranet_l = true " +
                                        "ORDER BY PUB_a_gift.p_donor_key_n";
                DataSet GiftBatchDS = DBAccess.GDBAccessObj.Select(GiftBatchQuery, "GiftBatchTbl", FTransaction);

                foreach (DataRow Row in GiftBatchDS.Tables["GiftBatchTbl"].Rows)
                {
                    Int32 RecipientKey = Convert.ToInt32(Row["RecipientKey"]);
                    Int32 RecipientFund = Batch.LedgerNumber;

                    // If no RecipientKey was specified, I need to get one:
                    if (RecipientKey == 0)
                    {
                        // If the Gift is on a foreign ledger,
                        // then RecipientKey is the PartnerKey of that ledger
                        ACostCentreTable CostCentreTbl = ACostCentreAccess.LoadByPrimaryKey(Batch.LedgerNumber,
                            Row["CostCentre"].ToString(), FTransaction);

                        if ((CostCentreTbl.Rows.Count == 1) && (CostCentreTbl[0].CostCentreType == "Foreign"))
                        {
                            AValidLedgerNumberTable ValidLedgerNumberTbl =
                                AValidLedgerNumberAccess.LoadViaACostCentre(Batch.LedgerNumber, Row["CostCentre"].ToString(), FTransaction);

                            if (ValidLedgerNumberTbl.Rows.Count == 1)
                            {
                                RecipientKey = Convert.ToInt32(ValidLedgerNumberTbl[0].PartnerKey);
                                RecipientFund = RecipientKey;
                            }
                        }
                    }

                    // Otherwise, I can derive a RecipientKey from the Ledger Number.
                    if (RecipientKey == 0)
                    {
                        RecipientKey = Batch.LedgerNumber * 1000000;
                        RecipientFund = RecipientKey;
                    }

                    GetDonor(Convert.ToInt32(Row["DonorKey"]));  // This adds the Donor to my list if it's not already present.
                    GetRecipient(RecipientKey); // This adds the recipient to my list if it's not already present.

                    decimal GIFFee;
                    decimal ICTFee;
                    decimal OtherFee;
                    GetAdminFees(Batch.LedgerNumber,
                        Convert.ToInt32(Row["BatchNumber"]), Convert.ToInt32(Row["TransactionNumber"]), Convert.ToInt32(Row["DetailNumber"]),
                        out GIFFee, out ICTFee, out OtherFee);
                    sw2.WriteLine(String.Format(
                            "{0:D10},{1:D10},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},\"{12},{13},{14},{15}\",{16:D10},{17:D10},{18},\"{19}\",\"{20}\",\"{21}\",\"{22}\",\"{23}\",\"{24}\"",
                            Row["DonorKey"],        //  0
                            RecipientKey,           //  1
                            Row["TransactionAmount"], //  2
                            Row["CurrencyCode"],    //  3
                            Row["GiftAmount"],      //  4
                            GIFFee,                 //  5
                            ICTFee,                 //  6
                            OtherFee,               //  7
                            Row["BaseCurrency"],    //  8
                            Row["IntlAmount"],      //  9
                            Row["IntlCurrency"],    // 10
                            PutDate(Row["EffectiveDate"]), // 11
                            Batch.LedgerNumber,     // 12
                            Row["BatchNumber"],     // 13
                            Row["TransactionNumber"], // 14
                            Row["DetailNumber"],    // 15
                            Batch.LedgerNumber * 1000000, // 16
                            RecipientFund,          // 17
                            Row["Confidential"],    // 18
                            Row["CommentOne"],      // 19
                            Row["CommentOneType"],  // 20
                            Row["CommentTwo"],      // 21
                            Row["CommentTwoType"],  // 22
                            Row["CommentThree"],    // 23
                            Row["CommentThreeType"] // 24
                            ));
                }
            }

            foreach (Int64 PartnerKey in DonorList.Keys)
            {
                PartnerDetails Row = DonorList[PartnerKey];
                sw1.WriteLine(String.Format("\"{0}\",\"{1}\",{2:D10},\"{3}\",\"{4}\",{5},{6},{7}",
                        Row.FirstName, Row.LastName, PartnerKey, Row.Email, Row.Address, Row.Telephone, Row.Anonymous ? "true" : "false", Row.Class));
            }

            sw1.Close();
            sw2.Close();

            StreamWriter sw3 = File.CreateText(FExportFilePath + "recipient.csv");
            sw3.WriteLine("recipient,first_name,last_name,email,class");

            foreach (Int64 PartnerKey in RecipientList.Keys)
            {
                PartnerDetails Row = RecipientList[PartnerKey];
                sw3.WriteLine(String.Format("{0},\"{1}\",\"{2}\",\"{3}\",{4}", PartnerKey, Row.FirstName, Row.LastName, Row.Email, Row.Class));
            }

            sw3.Close();

            return true;
        }

        private static Boolean ExportField()
        {
            Int64 MySiteKey = DomainManager.GSiteKey;

            /*
             *  From 4GL:
             *
             *  FIND FIRST pbfPartnerLocation WHERE p_partner_key_n = pdPartnerKey
             *      AND p_location_type_c = pcLocationType
             *      AND p_send_mail_l = plMailing
             *      AND (p_date_effective_d = ? OR p_date_effective_d <= TODAY)
             *      AND (p_date_good_until_d = ? OR p_date_good_until_d >= TODAY)
             *      NO-LOCK NO-ERROR.
             *  FIND pbfLocation OF pbfPartnerLocation NO-LOCK NO-ERROR.
             */
            String SqlQuery =
                "Select " +
                "PUB_p_location.p_building_1_c AS Building1, " +
                "PUB_p_location.p_building_2_c AS Building2, " +
                "PUB_p_location.p_street_name_c AS StreetName, " +
                "PUB_p_location.p_locality_c AS Locality, " +
                "PUB_p_location.p_suburb_c AS Suburb, " +
                "PUB_p_location.p_city_c AS City, " +
                "PUB_p_location.p_county_c AS County, " +
                "PUB_p_location.p_postal_code_c AS PostalCode, " +
                "PUB_p_location.p_country_code_c AS CountryCode, " +
                "PUB_p_location.p_address_3_c AS Address3, " +
                "PUB_p_partner_location.p_email_address_c AS Email, " +
                "PUB_p_partner_location.p_fax_number_c As Fax, " +
                "PUB_p_partner_location.p_url_c AS Website, " +
                "PUB_p_partner_location.p_telephone_number_c AS Telephone, " +
                "PUB_p_partner_location.p_send_mail_l AS SendMail, " +
                "PUB_p_country.p_time_zone_minimum_n AS TimeZoneMin, " +
                "PUB_p_country.p_time_zone_maximum_n AS TimeZoneMax, " +
                "PUB_p_country.p_internat_telephone_code_i AS InternationalPhone " +
                "FROM PUB_p_partner_location LEFT JOIN PUB_p_location ON PUB_p_partner_location.p_site_key_n = PUB_p_location.p_site_key_n " +
                "LEFT JOIN PUB_p_country ON PUB_p_location.p_country_code_c = PUB_p_country.p_country_code_c " +
                "WHERE PUB_p_partner_location.p_partner_key_n = " + MySiteKey.ToString("D10") +
                " AND (PUB_p_partner_location.p_location_type_c = 'BUSINESS') " +
                " AND (PUB_p_partner_location.p_date_effective_d IS NULL OR PUB_p_partner_location.p_date_effective_d < NOW()) " +
                " AND (PUB_p_partner_location.p_date_good_until_d IS NULL OR PUB_p_partner_location.p_date_good_until_d > NOW()) "
            ;

            DataSet FieldLocationDS = DBAccess.GDBAccessObj.Select(SqlQuery, "FieldLocationTbl", FTransaction);
            DataRow Row;

            if (FieldLocationDS.Tables["FieldLocationTbl"].Rows.Count == 0)
            {
                return false;
            }

            Row = FieldLocationDS.Tables["FieldLocationTbl"].Rows[0];
            String PostalAddress = Calculations.DetermineLocationString(
                Row["Building1"].ToString(), Row["Building2"].ToString(), Row["Locality"].ToString(), Row["StreetName"].ToString(),
                Row["Address3"].ToString(), Row["Suburb"].ToString(), Row["City"].ToString(), Row["County"].ToString(),
                Row["PostalCode"].ToString(), Row["CountryCode"].ToString(), Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
            //
            // If there's a "non-mailing" address, this is the "real" one, with email, fax, url etc.
            FieldLocationDS.Tables["FieldLocationTbl"].DefaultView.RowFilter = "SendMail=false";

            String StreetAddress;

            if (FieldLocationDS.Tables["FieldLocationTbl"].DefaultView.Count > 0)
            {
                Row = FieldLocationDS.Tables["FieldLocationTbl"].DefaultView[0].Row;
                StreetAddress = Calculations.DetermineLocationString(
                    Row["Building1"].ToString(), Row["Building2"].ToString(), Row["Locality"].ToString(), Row["StreetName"].ToString(),
                    Row["Address3"].ToString(), Row["Suburb"].ToString(), Row["City"].ToString(), Row["County"].ToString(),
                    Row["PostalCode"].ToString(), Row["CountryCode"].ToString(), Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
            }
            else
            {
                StreetAddress = PostalAddress;
            }

            String TimeZone = Row["TimeZoneMin"].ToString();

            if (Row["TimeZoneMin"] != Row["TimeZoneMax"])
            {
                TimeZone += (" " + Row["TimeZoneMax"].ToString());
            }

            String IntlPrefix = ("+" + Row["InternationalPhone"] + " ");
            using (StreamWriter sw = new StreamWriter(FExportFilePath + "field.csv"))
            {
                sw.WriteLine("key,value");
                sw.WriteLine("time_zone," + TimeZone);
                sw.WriteLine("postal_address,\"" + PostalAddress + "\"");
                sw.WriteLine("street_address,\"" + StreetAddress + "\"");
                sw.WriteLine("email," + Row["Email"]);
                sw.WriteLine("fax," + IntlPrefix + Row["Fax"]);
                sw.WriteLine("website," + Row["Website"]);
                sw.WriteLine("telephone," + IntlPrefix + Row["Telephone"]);
            }
            return true;
        }

        private static Boolean ExportPersonnel()
        {
            SortedList <Int64, PersonRec>PersonnelList = new SortedList <Int64, PersonRec>();
            using (StreamWriter sw = new StreamWriter(FExportFilePath + "position.csv"))
            {
                sw.WriteLine("person_key,role,field_key,start_date,end_date,assistant");

                String SqlQuery = "SELECT " +
                                  "PUB_pm_staff_data.p_partner_key_n AS person_key," +
                                  "PUB_pm_staff_data.pm_status_code_c AS role," +
                                  "PUB_pm_staff_data.pm_receiving_field_n AS field_key," +
                                  "PUB_pm_staff_data.pm_start_of_commitment_d AS start_date," +
                                  "PUB_pm_staff_data.pm_end_of_commitment_d AS end_date," +
                                  "PUB_p_person.p_title_c AS cTitle," +
                                  "PUB_p_person.p_first_name_c AS cFirstName," +
                                  "PUB_p_person.p_middle_name_1_c AS cMiddleName," +
                                  "PUB_p_person.p_family_name_c AS cLastName," +
                                  "PUB_p_person.p_academic_title_c AS cAcademic," +
                                  "PUB_p_person.p_decorations_c AS cDecorations," +
                                  "PUB_p_person.p_gender_c AS cGender," +
                                  "PUB_p_person.p_prefered_name_c AS cPreferredName," +
                                  "PUB_p_person.p_family_key_n AS dFamilyKey," +
                                  "PUB_pm_staff_data.pm_home_office_n AS dHomeOffice," +
                                  "PUB_p_person.p_date_of_birth_d AS dtBirthDate, " +
                                  "PUB_p_partner_location.p_email_address_c AS cEmailAddress," +
                                  "PUB_p_partner_location.p_location_type_c AS cLocationType " +

                                  "FROM PUB_pm_staff_data LEFT JOIN PUB_p_person ON PUB_pm_staff_data.p_partner_key_n = PUB_p_person.p_partner_key_n "
                                  +
                                  "LEFT JOIN PUB_p_partner_location ON (PUB_pm_staff_data.p_partner_key_n = PUB_p_partner_location.p_partner_key_n) "
                                  +
                                  "WHERE (PUB_p_partner_location.p_date_good_until_d IS NULL OR PUB_p_partner_location.p_date_good_until_d > NOW()) "
                                  +
                                  "AND PUB_pm_staff_data.pm_end_of_commitment_d IS NULL OR PUB_pm_staff_data.pm_end_of_commitment_d > NOW();";

                DataSet StaffPersonDS = DBAccess.GDBAccessObj.Select(SqlQuery, "StaffPersonTbl", FTransaction);

                // For each qualifying person in my PmStaffData, I'll produce a position.csv row,
                // and also produce unique rows for person.csv and email.csv.

                foreach (DataRow Row in StaffPersonDS.Tables["StaffPersonTbl"].Rows)
                {
                    String Role = "";

                    switch (Row["role"].ToString())
                    {
                        case "SHORT-TERMER": Role = "OM-OMER-GC"; break;

                        case "OMER": Role = "OM-OMER"; break;

                        case "LONG-TERMER": Role = "OM-OMER-LT"; break;

                        case "STAFF": Role = "OM-OMER-STAFF"; break;

                        case "GUEST": Role = "OM-GUEST"; break;

                        case "TRANSITION": Role = "TRANSITION"; break;

                        case "VOLUNTEER": continue;  // Don't export volunteers
                    }

                    // person_key,role,field_key,start_date,end_date,assistant
                    Int64 PersonKey = Convert.ToInt64(Row["person_key"]);

                    sw.WriteLine(String.Format("{0:D10},\"{1}\",{2:D10},\"{3}\",\"{4}\",FALSE",
                            PersonKey, Role, Convert.ToInt64(Row["field_key"]), PutDate(Row["start_date"]), PutDate(Row["end_date"])));

                    // Produce a unique row in my temporary list for person.csv and email.csv.
                    if (!PersonnelList.ContainsKey(PersonKey))
                    {
                        PersonRec PersonRow = new PersonRec();
                        PersonRow.Title = Row["cTitle"].ToString();
                        PersonRow.FirstName = Row["cFirstName"].ToString();
                        PersonRow.MiddleName = Row["cMiddleName"].ToString();
                        PersonRow.LastName = Row["cLastName"].ToString();
                        PersonRow.Academic = Row["cAcademic"].ToString();
                        PersonRow.Decorations = Row["cDecorations"].ToString();
                        PersonRow.Gender = Row["cGender"].ToString();
                        PersonRow.PreferredName = Row["cPreferredName"].ToString();
                        PersonRow.FamilyKey = Convert.ToInt64(Row["dFamilyKey"]);
                        PersonRow.HomeOffice = Convert.ToInt64(Row["dHomeOffice"]);

                        if (Row["dtBirthDate"].GetType() != typeof(System.DBNull))
                        {
                            PersonRow.DateOfBirth = (DateTime)Row["dtBirthDate"];
                        }

                        PersonRow.EmailAddress = Row["cEmailAddress"].ToString();
                        PersonRow.LocationType = Row["cLocationType"].ToString();

                        PersonnelList.Add(PersonKey, PersonRow);
                    }
                }
            }

            // person.csv
            using (StreamWriter sw = new StreamWriter(FExportFilePath + "person.csv"))
            {
                sw.WriteLine(
                    "title,first_name,middle_name,last_name,academic_title,decorations,gender,preferred_name,person_key,family_key,home_office,birth_date");

                foreach (Int64 PersonKey in PersonnelList.Keys)
                {
                    PersonRec PersonRow = PersonnelList[PersonKey];
                    sw.WriteLine(String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",{8:D10},{9:D10},{10:D10},\"{11}\"",
                            PersonRow.Title, PersonRow.FirstName, PersonRow.MiddleName, PersonRow.LastName,
                            PersonRow.Academic, PersonRow.Decorations, PersonRow.Gender, PersonRow.PreferredName,
                            PersonKey, PersonRow.FamilyKey, PersonRow.HomeOffice, PutDate(PersonRow.DateOfBirth)
                            ));
                }
            }

            // email.csv
            using (StreamWriter sw = new StreamWriter(FExportFilePath + "email.csv"))
            {
                sw.WriteLine("partner_key,email,type");

                foreach (Int64 PersonKey in PersonnelList.Keys)
                {
                    PersonRec PersonRow = PersonnelList[PersonKey];
                    sw.WriteLine(String.Format("{0:D10},\"{1}\",\"{2}\"",
                            PersonKey, PersonRow.EmailAddress, PersonRow.LocationType
                            ));
                }
            }

            TSystemDefaults.SetSystemDefault("LastCalebExportPersonnel", DateTime.Now.ToString(ExportDateFormat));

            return true;
        }

        private static Boolean ExportMetadata(String AOptionalMetadata, String APassword)
        {
            using (StreamWriter sw = new StreamWriter(FExportFilePath + "metadata.csv"))
            {
                sw.WriteLine("key,value");
                sw.WriteLine("version," + ExportVersion);
                sw.WriteLine("office," + ((Int64)DomainManager.GSiteKey).ToString("D10"));
                sw.WriteLine("date," + DateTime.Now.ToString(ExportDateFormat));
                sw.WriteLine("time," + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine("options," + AOptionalMetadata);
                sw.WriteLine("password,\"" + APassword + "\"");
            }

            return true;
        }

        private static void AddZipFile(String AFilename)
        {
            FExportTrace += ("    " + AFilename + "\r\n");
            FZipFileNames.Add(FExportFilePath + AFilename);
        }

        private static void DeleteTemporaryFiles()
        {
            foreach (String FullPath in FZipFileNames)
            {
                try
                {
                    File.Delete(FullPath);
                }
                catch (Exception)
                {
                }   // If I can't delete this file, I'll not worry just now (although it might become a problem later!)
            }

            FZipFileNames.Clear();

            File.Delete(FExportFilePath + "data.zip");
        }

        private static bool EncryptUsingPublicKey(
            String AFileName,
            String AEmailRecipient
            )
        {
            FExportTrace += ("\r\nEncrypt using PGP:");
            //
            // I'll shell out and call GPG, using a command line like this:
            // gpg -r tim.ingham@om.org -e data.zip
            //

            File.Delete(FExportFilePath + AFileName + ".gpg");
            System.Diagnostics.Process ShellProcess = new System.Diagnostics.Process();
            ShellProcess.EnableRaisingEvents = false;
            ShellProcess.StartInfo.Arguments = "-r " + AEmailRecipient + " -e " + FExportFilePath + AFileName;
            ShellProcess.StartInfo.FileName = "GPG.EXE";
            bool ExecOK = ShellProcess.Start();

            if (ExecOK)
            {
                ShellProcess.WaitForExit(10000);
                int ExecCode = ShellProcess.ExitCode;

                switch (ExecCode)
                {
                    case 0:
                        FExportTrace += "\r\nEncrypted to data.zip.gpg.";
                        break;

                    case 1:
                        FExportTrace += "\r\nERROR in GPG encryption Process.";
                        ExecOK = false;
                        break;

                    case 2:
                        FExportTrace += "\r\nERROR in GPG Command line.";
                        ExecOK = false;
                        break;

                    default:
                        FExportTrace += String.Format("\r\nUnknown ERROR code {0} in GPG Process.", ExecCode);
                        ExecOK = false;
                        break;
                }
            }
            else
            {
                FExportTrace += "\r\nERROR: Can't start GPG encryption Process.";
            }

            return ExecOK;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AExportDonationData"></param>
        /// <param name="AExportFieldData"></param>
        /// <param name="AExportPersonData"></param>
        /// <param name="AServerEmailAddress"></param>
        /// <param name="APswd"></param>
        /// <param name="ADaySpan"></param>
        /// <param name="AOptionalMetadata"></param>
        /// <param name="ReplyToEmail"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static String ExportToIntranet(Boolean AExportDonationData, Boolean AExportFieldData, Boolean AExportPersonData,
            String AServerEmailAddress, String APswd, Int32 ADaySpan, String AOptionalMetadata, String ReplyToEmail)
        {
            try
            {
                FZipFileNames.Clear();
                FExportFilePath = TAppSettingsManager.GetValue("Server.PathTemp") + @"\";
                FExportTrace = "Exporting (Temporary path: " + FExportFilePath + ")\r\n";
                FTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                if (AExportDonationData)
                {
                    ExportDonations(ADaySpan);
                    AddZipFile("donor.csv");
                    AddZipFile("donation.csv");
                    AddZipFile("recipient.csv");
                }

                if (AExportFieldData)
                {
                    if (ExportField())
                    {
                        AddZipFile("field.csv");
                    }
                }

                if (AExportPersonData)
                {
                    ExportPersonnel();
                    AddZipFile("position.csv");
                    AddZipFile("person.csv");
                    AddZipFile("email.csv");
                }

                ExportMetadata(AOptionalMetadata, APswd);
                AddZipFile("metadata.csv");

                MemoryStream ZippedStream = TFileHelper.Streams.Compression.DeflateFilesIntoMemoryStream(FZipFileNames.ToArray(), false, "");
                TFileHelper.Streams.FileHandling.SaveStreamToFile(ZippedStream, FExportFilePath + "data.zip");
                FExportTrace += "\r\nFiles compressed to " + FExportFilePath + "data.zip.";

                if (EncryptUsingPublicKey("data.zip", AServerEmailAddress))
                {
                    TSmtpSender SendMail = new TSmtpSender(
                        TUserDefaults.GetStringDefault("SmtpHost"),
                        TUserDefaults.GetInt16Default("SmtpPort"),
                        TUserDefaults.GetBooleanDefault("SmtpUseSsl"),
                        TUserDefaults.GetStringDefault("SmtpUser"),
                        TUserDefaults.GetStringDefault("SmtpPassword"),
                        "");
                    String SenderAddress = ReplyToEmail;

                    MailMessage msg = new MailMessage(SenderAddress,
                        AServerEmailAddress,
                        "Data from OpenPetra",
                        "Here is the latest data from my field.");

                    msg.Attachments.Add(new Attachment(FExportFilePath + "data.zip.gpg"));

                    if (SendMail.SendMessage(msg))
                    {
                        FExportTrace += ("\r\nEmail sent to " + msg.To[0].Address);
                    }
                    else
                    {
                        FExportTrace += ("\r\nNo Email was sent.");
                    }

                    msg.Dispose(); // If I don't call this, the attached files are still locked!
                }
                else
                {
                    FExportTrace += "\r\nError: Data encryption failed.";
                }
            }
            catch (Exception e)
            {
                FExportTrace += ("\r\nException: " + e.Message);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                DeleteTemporaryFiles();
                DonorList.Clear();
                RecipientList.Clear();  // These lists are static so they'll stick around for ever,
                                        // but I don't need to keep the data which is taking up memory.
            }
            return FExportTrace;
        }
    }
}