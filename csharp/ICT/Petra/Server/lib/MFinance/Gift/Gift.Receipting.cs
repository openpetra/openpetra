//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using System.Data;
using System.Data.Odbc;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Printing;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using System.IO;
using System.Collections.Generic;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    ///<summary>
    /// This connector allows creating the gift receipts
    ///</summary>
    public class TReceiptingWebConnector
    {
        /// <summary>
        /// create the annual gift receipts for all donors in the given year;
        /// returns several html documents, each in its own body tag; for printing with the HTML printer;
        /// TODO images are currently locally linked
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static string CreateAnnualGiftReceipts(Int32 ALedgerNumber, DateTime AStartDate, DateTime AEndDate, string AHTMLTemplate)
        {
            TLanguageCulture.LoadLanguageAndCulture();

            // get BaseCurrency
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ALedgerTable LedgerTable = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                ALedgerNumber,
                out typeofTable);
            string BaseCurrency = LedgerTable[0].BaseCurrency;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // get the local country code
            string LocalCountryCode = TAddressTools.GetCountryCodeFromSiteLedger(Transaction);

            // first get all donors in the given date range
            string SqlStmt = TDataBase.ReadSqlFile("Gift.ReceiptPrinting.GetDonors.sql");

            OdbcParameter[] parameters = new OdbcParameter[3];
            parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
            parameters[0].Value = ALedgerNumber;
            parameters[1] = new OdbcParameter("StartDate", OdbcType.Date);
            parameters[1].Value = AStartDate;
            parameters[2] = new OdbcParameter("EndDate", OdbcType.Date);
            parameters[2].Value = AEndDate;

            DataTable donorkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "DonorKeys", Transaction, parameters);

            string ResultDocument = "";

            foreach (DataRow donorrow in donorkeys.Rows)
            {
                Int64 donorKey = Convert.ToInt64(donorrow[0]);
                string donorName = donorrow[1].ToString();

                SqlStmt = TDataBase.ReadSqlFile("Gift.ReceiptPrinting.GetDonationsOfDonor.sql");
                parameters = new OdbcParameter[4];
                parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
                parameters[0].Value = ALedgerNumber;
                parameters[1] = new OdbcParameter("StartDate", OdbcType.Date);
                parameters[1].Value = AStartDate;
                parameters[2] = new OdbcParameter("EndDate", OdbcType.Date);
                parameters[2].Value = AEndDate;
                parameters[3] = new OdbcParameter("DonorKey", OdbcType.BigInt);
                parameters[3].Value = donorKey;

                // TODO: should we print each gift detail, or just one row per gift?
                DataTable donations = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "Donations", Transaction, parameters);

                if (donations.Rows.Count > 0)
                {
                    string letter = FormatLetter(donorKey, donorName, donations, BaseCurrency, AHTMLTemplate, LocalCountryCode, Transaction);

                    if (TFormLettersTools.AttachNextPage(ref ResultDocument, letter))
                    {
                        // TODO: store somewhere that the receipt has been printed?
                        // TODO also store each receipt with the donor in document management, and in contact management?
                    }
                }
            }

            TFormLettersTools.CloseDocument(ref ResultDocument);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return ResultDocument;
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
        /// Format the letter for the donor with all the gifts
        ///
        /// Can also used for a single receipt.
        /// </summary>
        /// <returns>One or more html documents, each in its own body tag, for printing with the HTML printer</returns>
        private static string FormatLetter(Int64 ADonorKey,
            string ADonorName,
            DataTable ADonations,
            string ABaseCurrency,
            string AHTMLTemplate,
            string ALedgerCountryCode,
            TDBTransaction ATransaction)
        {
            // get details of the donor, and best address

            PLocationTable Location;
            PPartnerLocationTable PartnerLocation;
            string CountryName;
            string EmailAddress;

            if (!TAddressTools.GetBestAddress(ADonorKey, out Location, out PartnerLocation, out CountryName, out EmailAddress, ATransaction))
            {
                return "";
            }

            string msg = AHTMLTemplate;

            if (ADonorName.Contains(","))
            {
                msg = msg.Replace("#TITLE", Calculations.FormatShortName(ADonorName, eShortNameFormat.eOnlyTitle));
            }
            else
            {
                // organisations have no title
                msg = msg.Replace("#TITLE", "");
            }

            msg = msg.Replace("#NAME", Calculations.FormatShortName(ADonorName, eShortNameFormat.eReverseWithoutTitle));
            msg = msg.Replace("#STREETNAME", GetStringOrEmpty(Location[0].StreetName));
            msg = msg.Replace("#LOCATION", GetStringOrEmpty(Location[0].Locality));
            msg = msg.Replace("#ADDRESS3", GetStringOrEmpty(Location[0].Address3));
            msg = msg.Replace("#BUILDING1", GetStringOrEmpty(Location[0].Building1));
            msg = msg.Replace("#BUILDING2", GetStringOrEmpty(Location[0].Building2));
            msg = msg.Replace("#CITY", GetStringOrEmpty(Location[0].City));
            msg = msg.Replace("#POSTALCODE", GetStringOrEmpty(Location[0].PostalCode));
            msg = msg.Replace("#DATE", DateTime.Now.ToString("d. MMMM yyyy"));

            // according to German Post, there is no country code in front of the post code
            // if country code is same for the address of the recipient and this office, then COUNTRYNAME is cleared
            if (GetStringOrEmpty(Location[0].CountryCode) != ALedgerCountryCode)
            {
                msg = msg.Replace("#COUNTRYNAME", CountryName);
            }
            else
            {
                msg = msg.Replace("#COUNTRYNAME", "");
            }

            // recognise detail lines automatically
            string RowTemplate;
            msg = TPrinterHtml.GetTableRow(msg, "#AMOUNT", out RowTemplate);
            string rowTexts = "";
            decimal sum = 0;

            decimal prevAmount = 0.0M;
            string prevCurrency = String.Empty;
            string prevCommentOne = String.Empty;
            string prevAccountDesc = String.Empty;
            string prevCostCentreDesc = String.Empty;
            string prevgifttype = string.Empty;
            DateTime prevDateEntered = DateTime.MaxValue;

            foreach (DataRow rowGifts in ADonations.Rows)
            {
                DateTime dateEntered = Convert.ToDateTime(rowGifts["DateEntered"]);
                decimal amount = Convert.ToDecimal(rowGifts["TransactionAmount"]);
                string currency = rowGifts["Currency"].ToString();
                string commentOne = rowGifts["CommentOne"].ToString();
                string accountDesc = rowGifts["AccountDesc"].ToString();
                string costcentreDesc = rowGifts["CostCentreDesc"].ToString();
                string gifttype = rowGifts["GiftType"].ToString();

                sum += Convert.ToDecimal(rowGifts["AmountInBaseCurrency"]);

                // can we sum up donations on the same date, or do we need to print each detail with the account description?
                if (RowTemplate.Contains("#COMMENTONE") || RowTemplate.Contains("#ACCOUNTDESC") || RowTemplate.Contains("#COSTCENTREDESC"))
                {
                    if (gifttype == MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND)
                    {
                        RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT);
                    }
                    else if (gifttype == MFinanceConstants.GIFT_TYPE_GIFT)
                    {
                        RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND);
                    }

                    rowTexts += RowTemplate.
                                Replace("#DONATIONDATE", dateEntered.ToString("dd.MM.yyyy")).
                                Replace("#AMOUNT", String.Format("{0:0.00}", amount)).
                                Replace("#AMOUNTCURRENCY", currency).
                                Replace("#COMMENTONE", commentOne).
                                Replace("#ACCOUNTDESC", accountDesc).
                                Replace("#COSTCENTREDESC", costcentreDesc);
                }
                else
                {
                    if ((dateEntered != prevDateEntered) && (prevDateEntered != DateTime.MaxValue))
                    {
                        if (prevgifttype == MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND)
                        {
                            RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT);
                        }
                        else if (prevgifttype == MFinanceConstants.GIFT_TYPE_GIFT)
                        {
                            RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND);
                        }

                        rowTexts += RowTemplate.
                                    Replace("#DONATIONDATE", prevDateEntered.ToString("dd.MM.yyyy")).
                                    Replace("#AMOUNT", String.Format("{0:0.00}", prevAmount)).
                                    Replace("#AMOUNTCURRENCY", prevCurrency).
                                    Replace("#COMMENTONE", prevCommentOne).
                                    Replace("#ACCOUNTDESC", prevAccountDesc).
                                    Replace("#COSTCENTREDESC", prevCostCentreDesc);
                        prevAmount = amount;
                    }
                    else
                    {
                        prevAmount += amount;
                    }

                    prevCurrency = currency;
                    prevDateEntered = dateEntered;
                    prevCommentOne = commentOne;
                    prevAccountDesc = accountDesc;
                    prevCostCentreDesc = costcentreDesc;
                    prevgifttype = gifttype;
                }
            }

            if (prevDateEntered != DateTime.MaxValue)
            {
                if (prevgifttype == MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND)
                {
                    RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT);
                }
                else if (prevgifttype == MFinanceConstants.GIFT_TYPE_GIFT)
                {
                    RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND);
                }

                rowTexts += RowTemplate.
                            Replace("#DONATIONDATE", prevDateEntered.ToString("dd.MM.yyyy")).
                            Replace("#AMOUNT", String.Format("{0:0.00}", prevAmount)).
                            Replace("#AMOUNTCURRENCY", prevCurrency).
                            Replace("#COMMENTONE", prevCommentOne).
                            Replace("#ACCOUNTDESC", prevAccountDesc).
                            Replace("#COSTCENTREDESC", prevCostCentreDesc);
                prevAmount = 0.0M;
            }

            msg = msg.Replace("#OVERALLAMOUNT", String.Format("{0:0.00}", sum)).
                  Replace("#OVERALLAMOUNTCURRENCY", ABaseCurrency);

            if ((ADonations.Rows.Count == 1) && msg.Contains("#DONATIONDATE"))
            {
                // this is a receipt for just one gift
                msg = msg.Replace("#DONATIONDATE", Convert.ToDateTime(ADonations.Rows[0]["DateEntered"]).ToString("dd.MM.yyyy"));
            }

            // TODO allow other currencies. use a_currency table, and base currency
            msg = msg.Replace("#TOTALAMOUNTINWORDS", NumberToWords.AmountToWords(sum, "Euro", "Cent"));

            return msg.Replace("#ROWTEMPLATE", rowTexts);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetUnreceiptedGifts(Int32 ALedgerNumber)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            DataTable GiftsTbl = null;

            try
            {
                String SqlQuery = "SELECT DISTINCT " +
                                  ALedgerNumber.ToString() + " As LedgerNumber," +
                                  "a_receipt_number_i AS ReceiptNumber," +
                                  "a_date_entered_d AS DateEntered," +
                                  "p_partner_short_name_c AS Donor," +
                                  "p_donor_key_n AS DonorKey," +
                                  "p_partner_class_c AS DonorClass," +
                                  "PUB_a_gift.a_batch_number_i AS BatchNumber," +
                                  "PUB_a_gift.a_gift_transaction_number_i AS TransactionNumber," +
                                  "a_reference_c AS Reference, " +
                                  "a_currency_code_c AS GiftCurrency " +
                                  "FROM PUB_a_gift LEFT JOIN PUB_p_partner on PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n " +
                                  "LEFT JOIN PUB_a_gift_batch ON PUB_a_gift.a_ledger_number_i = PUB_a_gift_batch.a_ledger_number_i AND PUB_a_gift.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i "
                                  +
                                  "WHERE PUB_a_gift.a_ledger_number_i=" + ALedgerNumber.ToString() +
                                  " AND a_receipt_printed_l=FALSE AND p_receipt_each_gift_l=TRUE " +
                                  "ORDER BY BatchNumber";

                GiftsTbl = DBAccess.GDBAccessObj.SelectDT(SqlQuery, "UnreceiptedGiftsTbl", Transaction);

                foreach (DataRow Row in GiftsTbl.Rows)
                {
                    Row["Donor"] = Calculations.FormatShortName(Row["Donor"].ToString(), eShortNameFormat.eReverseShortname);
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return GiftsTbl;
        }

        /// <summary>
        /// Produce a single page HTML letter to receipt a gift
        /// </summary>
        /// <param name="ADonorShortName"></param>
        /// <param name="ADonorKey"></param>
        /// <param name="ADonorClass"></param>
        /// <param name="AGiftCurrency"></param>
        /// <param name="ADateEntered"></param>
        /// <param name="ALocalCountryCode">If the addressee's country is the same as this, it won't be printed on the address label.</param>
        /// <param name="AGiftsThisDonor"></param>
        /// <param name="ATransaction">This can be read-only - nothing is written to the DB.</param>
        /// <returns>Complete (simple) HTML file</returns>
        [NoRemoting]
        public static string FormatHtmlReceipt(
            String ADonorShortName,
            Int64 ADonorKey,
            TPartnerClass ADonorClass,
            String AGiftCurrency,
            DateTime ADateEntered,
            string ALocalCountryCode,
            AGiftTable AGiftsThisDonor,
            TDBTransaction ATransaction)
        {
            SortedList <string, List <string>>FormValues = new SortedList <string, List <string>>();

            // These are the fields that can be printed in the letter:
            FormValues.Add("AdresseeShortName", new List <string>());
            FormValues.Add("AdresseeTitle", new List <string>());
            FormValues.Add("AdresseeFirstName", new List <string>());
            FormValues.Add("AdresseeFamilyName", new List <string>());
            FormValues.Add("AdresseeStreetAddress", new List <string>());
            FormValues.Add("AdresseeAddress3", new List <string>());
            FormValues.Add("AdresseeCity", new List <string>());
            FormValues.Add("AdresseePostCode", new List <string>());
            FormValues.Add("AdresseeCountry", new List <string>());
            FormValues.Add("FormattedAddress", new List <string>());

            FormValues.Add("DateToday", new List <string>());

            FormValues.Add("DateEntered", new List <string>());
            FormValues.Add("GiftAmount", new List <string>());
            FormValues.Add("GiftCurrency", new List <string>());
            FormValues.Add("GiftTxd", new List <string>());
            FormValues.Add("RecipientShortName", new List <string>());
            FormValues.Add("MotivationDetail", new List <string>());
            FormValues.Add("Reference", new List <string>());
            FormValues.Add("DonorComment", new List <string>());

            FormValues.Add("GiftTotalAmount", new List <string>());
            FormValues.Add("GiftTotalCurrency", new List <string>());
            FormValues.Add("TxdTotal", new List <string>());
            FormValues.Add("NonTxdTotal", new List <string>());


            // Donor Name:
            FormValues["AdresseeShortName"].Add(ADonorShortName);

            if (ADonorClass == TPartnerClass.PERSON)
            {
                PPersonTable Tbl = PPersonAccess.LoadByPrimaryKey(ADonorKey, ATransaction);

                if (Tbl.Rows.Count > 0)
                {
                    FormValues["AdresseeTitle"].Add(Tbl[0].Title);
                    FormValues["AdresseeFirstName"].Add(Tbl[0].FirstName);
                    FormValues["AdresseeFamilyName"].Add(Tbl[0].FamilyName);
                }
            }
            else if (ADonorClass == TPartnerClass.FAMILY)
            {
                PFamilyTable Tbl = PFamilyAccess.LoadByPrimaryKey(ADonorKey, ATransaction);

                if (Tbl.Rows.Count > 0)
                {
                    FormValues["AdresseeTitle"].Add(Tbl[0].Title);
                    FormValues["AdresseeFirstName"].Add(Tbl[0].FirstName);
                    FormValues["AdresseeFamilyName"].Add(Tbl[0].FamilyName);
                }
            }

            FormValues["DateToday"].Add(DateTime.Now.ToString("dd MMMM yyyy"));

            // Donor Adress:
            PLocationTable Location;
            PPartnerLocationTable PartnerLocation;
            string CountryName;
            string EmailAddress;

            if (TAddressTools.GetBestAddress(ADonorKey, out Location, out PartnerLocation, out CountryName, out EmailAddress, ATransaction))
            {
                PLocationRow LocRow = Location[0];
                FormValues["AdresseeStreetAddress"].Add(LocRow.StreetName);
                FormValues["AdresseeAddress3"].Add(LocRow.Address3);
                FormValues["AdresseeCity"].Add(LocRow.City);
                FormValues["AdresseePostCode"].Add(LocRow.PostalCode);

                if (LocRow.CountryCode != ALocalCountryCode)  // Don't add the Donor's country if it's also my country:
                {
                    FormValues["AdresseeCountry"].Add(CountryName);
                }
                else
                {
                    LocRow.CountryCode = "";
                }

                FormValues["FormattedAddress"].Add(Calculations.DetermineLocationString(LocRow,
                        Calculations.TPartnerLocationFormatEnum.plfHtmlLineBreak));
            }

            decimal GiftTotal = 0;
            decimal TxdTotal = 0;
            decimal NonTxdTotal = 0;

            // Details per gift:
            foreach (AGiftRow GiftRow in AGiftsThisDonor.Rows)
            {
                String DateEntered = ADateEntered.ToString("dd MMM yyyy");
                String GiftReference = GiftRow.Reference;
                AGiftDetailTable DetailTbl = AGiftDetailAccess.LoadViaAGift(
                    GiftRow.LedgerNumber, GiftRow.BatchNumber, GiftRow.GiftTransactionNumber, ATransaction);

                foreach (AGiftDetailRow DetailRow in DetailTbl.Rows)
                {
                    FormValues["Reference"].Add(GiftReference);
                    FormValues["DateEntered"].Add(DateEntered);
                    GiftReference = "";                         // Date and Reference are one-per-gift, not per detail
                    DateEntered = "";                           // so if this gift has several details, I'll blank the subsequent lines.

                    string DonorComment = "";
                    FormValues["GiftAmount"].Add(DetailRow.GiftAmount.ToString("0.00"));
                    FormValues["GiftCurrency"].Add(AGiftCurrency);
                    FormValues["MotivationDetail"].Add(DetailRow.MotivationDetailCode);
                    GiftTotal += DetailRow.GiftAmount;

                    if (DetailRow.TaxDeductable)
                    {
                        FormValues["GiftTxd"].Add("Y");
                        TxdTotal += DetailRow.GiftAmount;
                    }
                    else
                    {
                        FormValues["GiftTxd"].Add(" ");
                        NonTxdTotal += DetailRow.GiftAmount;
                    }

                    // Recipient Short Name:
                    PPartnerTable RecipientTbl = PPartnerAccess.LoadByPrimaryKey(DetailRow.RecipientKey, ATransaction);

                    if (RecipientTbl.Rows.Count > 0)
                    {
                        String ShortName = Calculations.FormatShortName(RecipientTbl[0].PartnerShortName, eShortNameFormat.eReverseShortname);
                        FormValues["RecipientShortName"].Add(ShortName);
                    }

                    if (DetailRow.CommentOneType == "Donor")
                    {
                        DonorComment += DetailRow.GiftCommentOne;
                    }

                    if (DetailRow.CommentTwoType == "Donor")
                    {
                        if (DonorComment != "")
                        {
                            DonorComment += "\r\n";
                        }

                        DonorComment += DetailRow.GiftCommentTwo;
                    }

                    if (DetailRow.CommentThreeType == "Donor")
                    {
                        if (DonorComment != "")
                        {
                            DonorComment += "\r\n";
                        }

                        DonorComment += DetailRow.GiftCommentThree;
                    }

                    if (DonorComment != "")
                    {
                        DonorComment = "Comment: " + DonorComment;
                    }

                    FormValues["DonorComment"].Add(DonorComment);
                } // foreach GiftDetail

            } // foreach Gift

            FormValues["GiftTotalAmount"].Add(GiftTotal.ToString("0.00"));
            FormValues["GiftTotalCurrency"].Add(AGiftCurrency);
            FormValues["TxdTotal"].Add(TxdTotal.ToString("0.00"));
            FormValues["NonTxdTotal"].Add(NonTxdTotal.ToString("0.00"));

            string PageHtml = TFormLettersTools.PrintSimpleHTMLLetter(
                TAppSettingsManager.GetValue("Formletters.Path") + "\\GiftReceipt.html", FormValues);
            return PageHtml;
        }

        /// <param name="AGiftCurrency"></param>
        /// <param name="ADateEntered"></param>
        /// <param name="ADonorShortName"></param>
        /// <param name="ADonorKey"></param>
        /// <param name="ADonorClass"></param>
        /// <param name="GiftsThisDonor"></param>
        /// <summary></summary>
        /// <returns>A Receipt formatted with HTML</returns>
        [RequireModulePermission("FINANCE-1")]
        public static string PrintGiftReceipt(
            String AGiftCurrency,
            DateTime ADateEntered,
            String ADonorShortName,
            Int64 ADonorKey,
            TPartnerClass ADonorClass,
            AGiftTable GiftsThisDonor
            )
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            string HtmlDoc;

            try
            {
                string LocalCountryCode = TAddressTools.GetCountryCodeFromSiteLedger(Transaction);
                HtmlDoc = FormatHtmlReceipt(
                    ADonorShortName,
                    ADonorKey,
                    ADonorClass,
                    AGiftCurrency,
                    ADateEntered,
                    LocalCountryCode,
                    GiftsThisDonor,
                    Transaction);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return HtmlDoc;
        }

        private class TempDonorInfo
        {
            public String DonorShortName;
            public TPartnerClass DonorClass;
            public String GiftCurrency;
            public DateTime DateEntered;
        }

        /// <summary>
        /// </summary>
        /// <param name="AGiftTbl">Custom table from GetUnreceiptedGifts, above</param>
        /// <returns>One or more HTML documents in a single string</returns>
        [RequireModulePermission("FINANCE-1")]
        public static string PrintReceipts(DataTable AGiftTbl)
        {
            string HtmlDoc = "";
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            SortedList <Int64, AGiftTable>GiftsPerDonor = new SortedList <Int64, AGiftTable>();
            SortedList <Int64, TempDonorInfo>DonorInfo = new SortedList <Int64, TempDonorInfo>();

            try
            {
                string LocalCountryCode = TAddressTools.GetCountryCodeFromSiteLedger(Transaction);

                foreach (DataRow Row in AGiftTbl.Rows)
                {
                    if (Row["Selected"].Equals(true))
                    {
                        Int64 DonorKey = Convert.ToInt64(Row["DonorKey"]);
                        //
                        // I need to merge any rows that have the same donor.
                        //

                        if (!GiftsPerDonor.ContainsKey(DonorKey))
                        {
                            GiftsPerDonor.Add(DonorKey, new AGiftTable());
                            DonorInfo.Add(DonorKey, new TempDonorInfo());
                        }

                        TempDonorInfo DonorRow = DonorInfo[DonorKey];
                        DonorRow.DonorShortName = Row["Donor"].ToString();
                        DonorRow.DonorClass = SharedTypes.PartnerClassStringToEnum(Row["DonorClass"].ToString());
                        DonorRow.GiftCurrency = Row["GiftCurrency"].ToString();
                        DonorRow.DateEntered = Convert.ToDateTime(Row["DateEntered"]);

                        AGiftRow GiftRow = GiftsPerDonor[DonorKey].NewRowTyped();
                        GiftRow.LedgerNumber = Convert.ToInt32(Row["LedgerNumber"]);
                        GiftRow.BatchNumber = Convert.ToInt32(Row["BatchNumber"]);
                        GiftRow.GiftTransactionNumber = Convert.ToInt32(Row["TransactionNumber"]);
                        GiftRow.Reference = Row["Reference"].ToString();
                        GiftsPerDonor[DonorKey].Rows.Add(GiftRow);
                    } // if Selected

                } // foreach Row

                foreach (Int64 DonorKey in GiftsPerDonor.Keys)
                {
                    TempDonorInfo DonorRow = DonorInfo[DonorKey];
                    string PageHtml = FormatHtmlReceipt(
                        DonorRow.DonorShortName,
                        DonorKey,
                        DonorRow.DonorClass,
                        DonorRow.GiftCurrency,
                        DonorRow.DateEntered,
                        LocalCountryCode,
                        GiftsPerDonor[DonorKey],
                        Transaction);

                    TFormLettersTools.AttachNextPage(ref HtmlDoc, PageHtml);
                } // foreach DonorKey

                TFormLettersTools.CloseDocument(ref HtmlDoc);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return HtmlDoc;
        }

        /// <summary>Mark a gift as receipted in the AGift table.</summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ATransactionNumber"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool MarkReceiptsPrinted(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 ATransactionNumber)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            AGiftTable Tbl = new AGiftTable();

            Tbl.Merge(AGiftAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, ATransactionNumber, Transaction));

            foreach (AGiftRow Row in Tbl.Rows)
            {
                Row.ReceiptPrinted = true;
            }

            TVerificationResultCollection SubmitResults;

            bool CommitRes = AGiftAccess.SubmitChanges(Tbl, Transaction, out SubmitResults);

            if (CommitRes)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return CommitRes;
        }

        /// <summary>Mark selected gifts as receipted in the AGift table.</summary>
        /// <param name="AGiftTbl">Custom DataTable from GetUnreceiptedGifts, above.
        /// For this method, only {bool}Selected, LedgerNumber, BatchNumber and TransactionNumber fields are needed.</param>
        [RequireModulePermission("FINANCE-1")]
        public static bool MarkReceiptsPrinted(DataTable AGiftTbl)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            AGiftTable Tbl = new AGiftTable();

            foreach (DataRow Row in AGiftTbl.Rows)
            {
                if (Row["Selected"].Equals(true))
                {
                    Tbl.Merge(AGiftAccess.LoadByPrimaryKey(
                            Convert.ToInt32(Row["LedgerNumber"]),
                            Convert.ToInt32(Row["BatchNumber"]),
                            Convert.ToInt32(Row["TransactionNumber"]),
                            Transaction));
                }
            }

            foreach (AGiftRow Row in Tbl.Rows)
            {
                Row.ReceiptPrinted = true;
            }

            TVerificationResultCollection SubmitResults;

            bool CommitRes = AGiftAccess.SubmitChanges(Tbl, Transaction, out SubmitResults);

            if (CommitRes)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return CommitRes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Int32 GetLastReceiptNumber(Int32 ALedgerNumber)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            ALedgerTable LedgerTbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            if (LedgerTbl.Rows.Count > 0)
            {
                return LedgerTbl[0].LastHeaderRNumber;
            }
            else
            {
                return 0; // This is obviously the wrong answer, but I judge it to be unlikely.
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static void SetLastReceiptNumber(Int32 ALedgerNumber, Int32 AReceiptNumber)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            ALedgerTable LedgerTbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            TVerificationResultCollection Results;

            if (LedgerTbl.Rows.Count > 0)
            {
                LedgerTbl[0].LastHeaderRNumber = AReceiptNumber;
                ALedgerAccess.SubmitChanges(LedgerTbl, Transaction, out Results);
            }

            DBAccess.GDBAccessObj.CommitTransaction();
        }
    }
}