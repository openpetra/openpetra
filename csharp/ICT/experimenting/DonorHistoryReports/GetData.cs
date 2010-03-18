/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.IO;
using Mono.Unix;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Printing;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;

namespace Ict.Petra.Client.MFinance.Gui.DonorHistoryReports
{
    /// <summary>
    /// retrieve data from the database
    /// </summary>
    public class TGetData
    {
        private static TDataBase db;

        /// open the db connection
        public static bool InitDBConnection()
        {
            // establish connection to database
            TAppSettingsManager settings = new TAppSettingsManager(false);

            db = new TDataBase();

            new TLogging("debug.log");
            db.DebugLevel = settings.GetInt16("DebugLevel", 0);

            TDBType dbtype = CommonTypes.ParseDBType(settings.GetValue("Server.RDBMSType"));

            if (dbtype != TDBType.ProgressODBC)
            {
                throw new Exception("at the moment only Progress ODBC db is supported");
            }

            db.EstablishDBConnection(dbtype,
                settings.GetValue("Server.ODBC_DSN"),
                "",
                settings.GetValue("odbc.username"),
                settings.GetValue("odbc.password"),
                "");
            DBAccess.GDBAccessObj = db;

            return true;
        }

        /// <summary>
        /// return a table with the details of people that have given according to the parameters
        /// </summary>
        public static void GetDonorHistory(ref DonorHistoryTDS AMainDS, DateTime ADateFrom, DateTime ADateUntil,
            Decimal AMinimumAmount, Decimal AMaximumAmount, Int32 AMinimumCount, Int32 AMaximumCount,
            bool AGiftsForProjects, bool AGiftsForSupport,
            bool ADonorFamily, bool ADonorChurch, bool ADonorOrganisation)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            string stmt = TDataBase.ReadSqlFile("GetDonorHistory.sql");

            OdbcParameter[] parameters = new OdbcParameter[4];
            parameters[0] = new OdbcParameter("StartDate", OdbcType.Date);
            parameters[0].Value = ADateFrom;
            parameters[1] = new OdbcParameter("EndDate", OdbcType.Date);
            parameters[1].Value = ADateUntil;
            parameters[2] = new OdbcParameter("MinimumAmount", OdbcType.Decimal);
            parameters[2].Value = AMinimumAmount;
            parameters[3] = new OdbcParameter("MaximumAmount", OdbcType.Decimal);
            parameters[3].Value = AMaximumAmount;

            AMainDS.DisableConstraints();

            DBAccess.GDBAccessObj.Select(AMainDS, stmt, AMainDS.Gift.TableName, transaction, parameters);

            DBAccess.GDBAccessObj.RollbackTransaction();

            // get recipient description
            foreach (DonorHistoryTDSGiftRow row in AMainDS.Gift.Rows)
            {
                if (row.RecipientDescription.Length == 0)
                {
                    row.RecipientDescription = row.MotivationGroupCode + "/" + row.MotivationDetailCode;
                }
            }

            AddUpDonations(AMainDS);

            // remove all donors with different number of gifts
            RemoveDonorsNotMatchingGiftCount(AMainDS, AMinimumCount, AMaximumCount);

            // best address for each partner
            AddPostalAddress(AMainDS.Donor, DonorHistoryTDSDonorTable.GetDonorKeyDBName());
        }

        /// <summary>
        /// count the number of gifts and populate donor's table.
        /// </summary>
        /// <param name="AMainDS"></param>
        private static void AddUpDonations(DonorHistoryTDS AMainDS)
        {
            AMainDS.Gift.DefaultView.Sort = DonorHistoryTDSGiftTable.GetDonorKeyDBName();

            DonorHistoryTDSDonorRow donorRow = null;

            foreach (DataRowView rv in AMainDS.Gift.DefaultView)
            {
                DonorHistoryTDSGiftRow row = (DonorHistoryTDSGiftRow)rv.Row;

                if ((donorRow == null) || (row.DonorKey != donorRow.DonorKey))
                {
                    donorRow = AMainDS.Donor.NewRowTyped();
                    donorRow.DonorKey = row.DonorKey;
                    donorRow.DonorShortName = row.DonorShortName;
                    donorRow.GiftTotalAmount = 0;
                    donorRow.GiftTotalCount = 0;
                    AMainDS.Donor.Rows.Add(donorRow);
                }

                donorRow.GiftTotalAmount += row.GiftAmount;
                donorRow.GiftTotalCount++;
            }
        }

        private static void RemoveDonorsNotMatchingGiftCount(DonorHistoryTDS AMainDS, Int32 AMinimumCount, Int32 AMaximumCount)
        {
            bool WasDeleted = true;

            while (WasDeleted)
            {
                WasDeleted = false;

                foreach (DonorHistoryTDSDonorRow row in AMainDS.Donor.Rows)
                {
                    if ((row.GiftTotalCount < AMinimumCount) || (row.GiftTotalCount > AMaximumCount))
                    {
                        row.Delete();
                        WasDeleted = true;

                        // work around changing the loop list by deleting the current element
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// get the postal address of the partners and add to the table
        /// </summary>
        private static void AddPostalAddress(DataTable ResultTable, string PartnerKeyColumnName)
        {
            if (!ResultTable.Columns.Contains("Email"))
            {
                ResultTable.Columns.Add("Email", typeof(string));
                ResultTable.Columns.Add("ValidAddress", typeof(bool));
                ResultTable.Columns.Add("Locality", typeof(string));
                ResultTable.Columns.Add("StreetName", typeof(string));
                ResultTable.Columns.Add("Building1", typeof(string));
                ResultTable.Columns.Add("Building2", typeof(string));
                ResultTable.Columns.Add("Address3", typeof(string));
                ResultTable.Columns.Add("CountryCode", typeof(string));
                ResultTable.Columns.Add("CountryName", typeof(string));
                ResultTable.Columns.Add("PostalCode", typeof(string));
                ResultTable.Columns.Add("City", typeof(string));
            }

            foreach (DataRowView rv in ResultTable.DefaultView)
            {
                DataRow row = rv.Row;

                if (row[PartnerKeyColumnName] != DBNull.Value)
                {
                    PLocationTable Address;
                    string CountryNameLocal;
                    string emailAddress = GetBestEmailAddress(Convert.ToInt64(row[PartnerKeyColumnName]), out Address, out CountryNameLocal);

                    if (emailAddress.Length > 0)
                    {
                        row["Email"] = emailAddress;
                    }

                    row["ValidAddress"] = (Address != null);

                    if (Address == null)
                    {
                        // no best address; only report if emailAddress is empty as well???
                        continue;
                    }

                    if (!Address[0].IsLocalityNull())
                    {
                        row["Locality"] = Address[0].Locality;
                    }

                    if (!Address[0].IsStreetNameNull())
                    {
                        row["StreetName"] = Address[0].StreetName;
                    }

                    if (!Address[0].IsBuilding1Null())
                    {
                        row["Building1"] = Address[0].Building1;
                    }

                    if (!Address[0].IsBuilding2Null())
                    {
                        row["Building2"] = Address[0].Building2;
                    }

                    if (!Address[0].IsAddress3Null())
                    {
                        row["Address3"] = Address[0].Address3;
                    }

                    if (!Address[0].IsCountryCodeNull())
                    {
                        row["CountryCode"] = Address[0].CountryCode;
                    }

                    row["CountryName"] = CountryNameLocal;

                    if (!Address[0].IsPostalCodeNull())
                    {
                        row["PostalCode"] = Address[0].PostalCode;
                    }

                    if (!Address[0].IsCityNull())
                    {
                        row["City"] = Address[0].City;
                    }
                }
            }
        }

        private static string GetBestEmailAddress(Int64 APartnerKey, out PLocationTable AAddress, out string ACountryNameLocal)
        {
            string EmailAddress = "";
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            AAddress = null;
            ACountryNameLocal = "";

            DataSet PartnerLocationsDS = new DataSet();

            PartnerLocationsDS.Tables.Add(new PPartnerLocationTable());
            PartnerLocationsDS.Tables.Add(new PCountryTable());
            DataTable PartnerLocationTable = PartnerLocationsDS.Tables[PPartnerLocationTable.GetTableName()];
            PCountryTable CountryTable = (PCountryTable)PartnerLocationsDS.Tables[PCountryTable.GetTableName()];
            CountryTable.DefaultView.Sort = PCountryTable.GetCountryCodeDBName();

            // add special column BestAddress and Icon
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            // find all locations of the partner, put it into a dataset
            PPartnerLocationAccess.LoadViaPPartner(PartnerLocationsDS, APartnerKey, Transaction);

            Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
            Ict.Petra.Shared.MPartner.Calculations.DetermineBestAddress(PartnerLocationsDS);

            foreach (PPartnerLocationRow row in PartnerLocationTable.Rows)
            {
                // find the row with BestAddress = 1
                if (Convert.ToInt32(row["BestAddress"]) == 1)
                {
                    if (!row.IsEmailAddressNull())
                    {
                        EmailAddress = row.EmailAddress;
                    }

                    // we also want the post address, need to load the p_location table:
                    AAddress = PLocationAccess.LoadByPrimaryKey(row.SiteKey, row.LocationKey, Transaction);

                    if (CountryTable.DefaultView.Find(AAddress[0].CountryCode) == -1)
                    {
                        CountryTable.Merge(PCountryAccess.LoadByPrimaryKey(AAddress[0].CountryCode, Transaction));
                    }

                    ACountryNameLocal = CountryTable[CountryTable.DefaultView.Find(AAddress[0].CountryCode)].CountryNameLocal;
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            return EmailAddress;
        }

        private static string GetStringOrEmpty(object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return "";
            }

            return obj.ToString();
        }

        private static string FormalGreeting(string APartnerShortName)
        {
            // TODO: use formal greetings from database, etc
            string title = Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlyTitle);

            if ((title.ToLower().Contains("herr") && title.ToLower().Contains("frau")) || title.ToLower().Contains("familie"))
            {
                return "Sehr geehrte " + title + " " + Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname);
            }
            else if (title.ToLower().Contains("herr"))
            {
                return "Sehr geehrter " + title + " " + Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname);
            }
            else if (title.ToLower().Contains("frau"))
            {
                return "Sehr geehrte " + title + " " + Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname);
            }
            else
            {
                return "Sehr geehrte(r) " + title + " " + Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname);
            }
        }

        /// <summary>
        /// prepare HTML text for each new donor
        /// </summary>
        public static List <string>PrepareLetters(ref DonorHistoryTDS AMainDS)
        {
            string LedgerCountryCode = TAppSettingsManager.GetValueStatic("Local.CountryCode");

            List <string>Letters = new List <string>();

            string letterTemplateFilename = TAppSettingsManager.GetValueStatic("LetterTemplate.File");

            // message body from HTML template
            StreamReader reader = new StreamReader(letterTemplateFilename);

            string templateMsg = reader.ReadToEnd();

            reader.Close();

            foreach (DonorHistoryTDSDonorRow row in AMainDS.Donor.Rows)
            {
                string donorName = row["DonorShortName"].ToString();

                if (Convert.ToBoolean(row["ValidAddress"]) == false)
                {
                    continue;
                }

                string msg = templateMsg;

                msg =
                    msg.Replace("#RECIPIENTNAME",
                        Calculations.FormatShortName(row["RecipientDescription"].ToString(), eShortNameFormat.eReverseWithoutTitle));
                msg = msg.Replace("#TITLE", Calculations.FormatShortName(donorName, eShortNameFormat.eOnlyTitle));
                msg = msg.Replace("#NAME", Calculations.FormatShortName(donorName, eShortNameFormat.eReverseWithoutTitle));
                msg = msg.Replace("#FORMALGREETING", FormalGreeting(donorName));
                msg = msg.Replace("#STREETNAME", GetStringOrEmpty(row["StreetName"]));
                msg = msg.Replace("#LOCATION", GetStringOrEmpty(row["Locality"]));
                msg = msg.Replace("#ADDRESS3", GetStringOrEmpty(row["Address3"]));
                msg = msg.Replace("#BUILDING1", GetStringOrEmpty(row["Building1"]));
                msg = msg.Replace("#BUILDING2", GetStringOrEmpty(row["Building2"]));
                msg = msg.Replace("#CITY", GetStringOrEmpty(row["City"]));
                msg = msg.Replace("#POSTALCODE", GetStringOrEmpty(row["PostalCode"]));
                msg = msg.Replace("#DATE", DateTime.Now.ToString("d. MMMM yyyy"));

                // according to German Post, there is no country code in front of the post code
                // if country code is same for the address of the recipient and this office, then COUNTRYNAME is cleared
                if (GetStringOrEmpty(row["CountryCode"]) != LedgerCountryCode)
                {
                    msg = msg.Replace("#COUNTRYNAME", GetStringOrEmpty(row["CountryName"]));
                }
                else
                {
                    msg = msg.Replace("#COUNTRYNAME", "");
                }

                // TODO: projects have names as well. different way to determine project gifts? motivation detail?
                if ((row["MotivationGroupCode"].ToString().ToUpper() == "GIFT") && (row["MotivationDetailCode"].ToString().ToUpper() == "SUPPORT"))
                {
                    msg = TPrinterHtml.RemoveDivWithClass(msg, "donationForProject");
                }
                else
                {
                    msg = TPrinterHtml.RemoveDivWithClass(msg, "donationForWorker");
                }

                Letters.Add(msg);
            }

            return Letters;
        }
    }
}