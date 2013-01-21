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
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Printing;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    /// <summary>
    /// a letter for a new donor telling him he can get a free subscription
    /// </summary>
    public class TNewDonorSubscriptionsWebConnector
    {
        /// <summary>
        /// return a table with the details of people that have a new subscriptions because they donated
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static NewDonorTDS GetNewDonorSubscriptions(
            string APublicationCode,
            DateTime ASubscriptionStartFrom,
            DateTime ASubscriptionStartUntil,
            string AExtractName,
            bool ADropForeignAddresses)
        {
            NewDonorTDS MainDS = new NewDonorTDS();

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadUncommitted, out NewTransaction);

            try
            {
                string stmt = TDataBase.ReadSqlFile("Gift.NewDonorSubscription.sql");

                OdbcParameter parameter;

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                parameter = new OdbcParameter("PublicationCode", OdbcType.VarChar);
                parameter.Value = APublicationCode;
                parameters.Add(parameter);
                parameter = new OdbcParameter("StartDate", OdbcType.Date);
                parameter.Value = ASubscriptionStartFrom;
                parameters.Add(parameter);
                parameter = new OdbcParameter("EndDate", OdbcType.Date);
                parameter.Value = ASubscriptionStartUntil;
                parameters.Add(parameter);
                parameter = new OdbcParameter("ExtractName", OdbcType.VarChar);
                parameter.Value = AExtractName;
                parameters.Add(parameter);

                DBAccess.GDBAccessObj.Select(MainDS, stmt, MainDS.AGift.TableName, transaction, parameters.ToArray());

                // drop all previous gifts, keep only the most recent one
                NewDonorTDSAGiftRow previousRow = null;

                Int32 rowCounter = 0;

                while (rowCounter < MainDS.AGift.Rows.Count)
                {
                    NewDonorTDSAGiftRow row = MainDS.AGift[rowCounter];

                    if ((previousRow != null) && (previousRow.DonorKey == row.DonorKey))
                    {
                        MainDS.AGift.Rows.Remove(previousRow);
                    }
                    else
                    {
                        rowCounter++;
                    }

                    previousRow = row;
                }

                // get recipient description
                foreach (NewDonorTDSAGiftRow row in MainDS.AGift.Rows)
                {
                    if (row.RecipientDescription.Length == 0)
                    {
                        row.RecipientDescription = row.MotivationGroupCode + "/" + row.MotivationDetailCode;
                    }
                }

                // best address for each partner
                DataUtilities.CopyTo(
                    TAddressTools.AddPostalAddress(MainDS.AGift, MainDS.AGift.ColumnDonorKey, ADropForeignAddresses, true, transaction),
                    MainDS.BestAddress);

                // remove all invalid addresses
                rowCounter = 0;

                while (rowCounter < MainDS.BestAddress.Rows.Count)
                {
                    BestAddressTDSLocationRow row = MainDS.BestAddress[rowCounter];

                    if (!row.ValidAddress)
                    {
                        MainDS.BestAddress.Rows.Remove(row);
                    }
                    else
                    {
                        rowCounter++;
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return MainDS;
        }

        private static string GetStringOrEmpty(object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return "";
            }

            return obj.ToString();
        }

        /// <summary>
        /// prepare HTML text for each new donor
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static StringCollection PrepareNewDonorLetters(ref NewDonorTDS AMainDS, string AHTMLTemplate)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            // get the local country code
            string LocalCountryCode = TAddressTools.GetCountryCodeFromSiteLedger(transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            StringCollection Letters = new StringCollection();

            foreach (BestAddressTDSLocationRow addressRow in AMainDS.BestAddress.Rows)
            {
                if (addressRow.ValidAddress == false)
                {
                    continue;
                }

                AMainDS.AGift.DefaultView.RowFilter = NewDonorTDSAGiftTable.GetDonorKeyDBName() + " = '" + addressRow.PartnerKey.ToString() + "'";
                NewDonorTDSAGiftRow row = (NewDonorTDSAGiftRow)AMainDS.AGift.DefaultView[0].Row;

                string donorName = row.DonorShortName;

                string msg = AHTMLTemplate;

                msg =
                    msg.Replace("#RECIPIENTNAME",
                        Calculations.FormatShortName(row.RecipientDescription, eShortNameFormat.eReverseWithoutTitle));
                msg =
                    msg.Replace("#RECIPIENTFIRSTNAME",
                        Calculations.FormatShortName(row.RecipientDescription, eShortNameFormat.eOnlyFirstname));
                msg = msg.Replace("#TITLE", Calculations.FormatShortName(donorName, eShortNameFormat.eOnlyTitle));
                msg = msg.Replace("#NAME", Calculations.FormatShortName(donorName, eShortNameFormat.eReverseWithoutTitle));
                msg = msg.Replace("#FORMALGREETING", Calculations.FormalGreeting(donorName));
                msg = msg.Replace("#STREETNAME", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnStreetNameId]));
                msg = msg.Replace("#LOCATION", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnLocalityId]));
                msg = msg.Replace("#ADDRESS3", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnAddress3Id]));
                msg = msg.Replace("#BUILDING1", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnBuilding1Id]));
                msg = msg.Replace("#BUILDING2", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnBuilding2Id]));
                msg = msg.Replace("#CITY", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnCityId]));
                msg = msg.Replace("#POSTALCODE", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnPostalCodeId]));
                msg = msg.Replace("#DATE", DateTime.Now.ToString("d. MMMM yyyy"));

                // according to German Post, there is no country code in front of the post code
                // if country code is same for the address of the recipient and this office, then COUNTRYNAME is cleared
                if (GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnCountryCodeId]) != LocalCountryCode)
                {
                    msg = msg.Replace("#COUNTRYNAME", GetStringOrEmpty(addressRow[BestAddressTDSLocationTable.ColumnCountryNameId]));
                }
                else
                {
                    msg = msg.Replace("#COUNTRYNAME", "");
                }

                // TODO: projects have names as well. different way to determine project gifts? motivation detail?
                if ((row.MotivationGroupCode.ToUpper() == "GIFT") && (row.MotivationDetailCode.ToUpper() == "SUPPORT"))
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