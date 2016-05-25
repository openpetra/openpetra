//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2013 by OM International
// Copyright 2013-2014 by SolidCharity
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.IO;

using GNU.Gettext;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Printing;

using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;

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
        public static string CreateAnnualGiftReceipts(Int32 ALedgerNumber,
            string AFrequency,
            DateTime AStartDate,
            DateTime AEndDate,
            string AHTMLTemplate,
            bool ADeceasedFirst = false,
            string AExtract = null,
            Int64 ADonorKey = 0)
        {
            string ResultDocument = string.Empty;

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

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    try
                    {
                        // get the local country code
                        string LocalCountryCode = TAddressTools.GetCountryCodeFromSiteLedger(Transaction);
                        DataTable donorkeys = new DataTable();
                        string SqlStmt = "";

                        if (ADonorKey != 0)
                        {
                            TPartnerClass Class;
                            string ShortName;
                            TPartnerServerLookups.GetPartnerShortName(ADonorKey, out ShortName, out Class);

                            donorkeys.Columns.Add(new DataColumn("DonorKey"));
                            donorkeys.Columns.Add(new DataColumn("DonorName"));
                            DataRow SingleRow = donorkeys.NewRow();
                            SingleRow[0] = ADonorKey;
                            SingleRow[1] = ShortName;

                            donorkeys.Rows.Add(SingleRow);
                        }
                        else
                        {
                            SortedList <string, string>Defines = new SortedList <string, string>();

                            if (!string.IsNullOrEmpty(AExtract))
                            {
                                Defines.Add("BYEXTRACT", string.Empty);
                            }

                            // first get all donors in the given date range
                            SqlStmt = TDataBase.ReadSqlFile("Gift.ReceiptPrinting.GetDonors.sql", Defines);

                            OdbcParameter[] parameters = new OdbcParameter[6];
                            parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
                            parameters[0].Value = ALedgerNumber;
                            parameters[1] = new OdbcParameter("StartDate", OdbcType.Date);
                            parameters[1].Value = AStartDate;
                            parameters[2] = new OdbcParameter("EndDate", OdbcType.Date);
                            parameters[2].Value = AEndDate;
                            parameters[3] = new OdbcParameter("IgnoreFrequency", OdbcType.Bit);

                            if (AFrequency == "")
                            {
                                parameters[3].Value = true;
                            }
                            else
                            {
                                parameters[3].Value = false;
                            }

                            parameters[4] = new OdbcParameter("Frequency", OdbcType.VarChar);
                            parameters[4].Value = AFrequency;
                            parameters[5] = new OdbcParameter("Extract", OdbcType.VarChar);
                            parameters[5].Value = AExtract;

                            donorkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "DonorKeys", Transaction, parameters);

                            // put deceased partner's at the front (still sorted alphabetically)
                            if (ADeceasedFirst)
                            {
                                // create a new datatable with same structure as donorkeys
                                DataTable temp = donorkeys.Clone();
                                temp.Clear();

                                // add deceased donors to the temp table and delete from donorkeys
                                for (int i = 0; i < donorkeys.Rows.Count; i++)
                                {
                                    if (SharedTypes.StdPartnerStatusCodeStringToEnum(donorkeys.Rows[i][2].ToString()) ==
                                        TStdPartnerStatusCode.spscDIED)
                                    {
                                        temp.Rows.Add((object[])donorkeys.Rows[i].ItemArray.Clone());
                                        donorkeys.Rows[i].Delete();
                                    }
                                }

                                // add remaining partners to temp table
                                donorkeys.AcceptChanges();
                                temp.Merge(donorkeys);

                                donorkeys = temp;
                            }
                        }

                        SqlStmt = TDataBase.ReadSqlFile("Gift.ReceiptPrinting.GetDonationsOfDonor.sql");

                        foreach (DataRow donorrow in donorkeys.Rows)
                        {
                            Int64 donorKey = Convert.ToInt64(donorrow[0]);
                            string donorName = donorrow[1].ToString();

                            OdbcParameter[] parameters = new OdbcParameter[4];
                            parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
                            parameters[0].Value = ALedgerNumber;
                            parameters[1] = new OdbcParameter("StartDate", OdbcType.Date);
                            parameters[1].Value = AStartDate;
                            parameters[2] = new OdbcParameter("EndDate", OdbcType.Date);
                            parameters[2].Value = AEndDate;
                            parameters[3] = new OdbcParameter("DonorKey", OdbcType.BigInt);
                            parameters[3].Value = donorKey;

                            DataTable donations = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "Donations", Transaction, parameters);

                            if (donations.Rows.Count > 0)
                            {
                                string letter =
                                    FormatLetter(donorKey, donorName, donations, BaseCurrency, AHTMLTemplate, LocalCountryCode, -1, true, Transaction);

                                if (TFormLettersTools.AttachNextPage(ref ResultDocument, letter))
                                {
                                    // TODO: store somewhere that the receipt has been printed?
                                    // TODO also store each receipt with the donor in document management, and in contact management?
                                }
                            }
                        }

                        TFormLettersTools.CloseDocument(ref ResultDocument);
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });

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
            Decimal AMinimumAmount,
            bool AAlwaysPrintNewDonor,
            TDBTransaction ATransaction)
        {
            // get details of the donor, and best address

            PLocationTable Location;
            string CountryName;

            string MajorUnitSingular = string.Empty;
            string MajorUnitPlural = string.Empty;
            string MinorUnitSingular = string.Empty;
            string MinorUnitPlural = string.Empty;

            if (!TAddressTools.GetBestAddress(ADonorKey, out Location, out CountryName, ATransaction))
            {
                return "";
            }

            bool TaxDeductiblePercentageEnabled =
                TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

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
            msg = msg.Replace("#DONORKEY", ADonorKey.ToString("0000000000"));

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
            string OrigRowTemplate = RowTemplate;
            string rowTexts = "";
            decimal sum = 0;
            decimal sumTaxDeduct = 0;
            decimal sumNonDeduct = 0;

            decimal prevAmount = 0.0M;
            decimal prevAmountTaxDeduct = 0.0M;
            decimal prevAmountNonDeduct = 0.0M;
            string prevCurrency = String.Empty;
            string prevgifttype = string.Empty;
            DateTime prevDateEntered = DateTime.MaxValue;

            // Find out which rows should not be included (if they are below minimum amount).
            // Attention: for split gifts check if sum of split gift is below minimum amount (not individual split gift).
            // this check does not need to be done if no minimum amount is requested.
            if ((AMinimumAmount > 0)
                && !AAlwaysPrintNewDonor)
            {
                decimal TotalGiftBaseAmount = 0;
                List <DataRow>deletedRows = new List <DataRow>();
                List <DataRow>tempDeletedRows = new List <DataRow>();

                foreach (DataRow rowGifts in ADonations.Rows)
                {
                    decimal BaseAmount = Convert.ToDecimal(rowGifts["AmountInBaseCurrency"]);
                    Int32 DetailNumber = Convert.ToInt32(rowGifts["DetailNumber"]);

                    if (DetailNumber > 1)
                    {
                        // part of split gift
                        TotalGiftBaseAmount += BaseAmount;
                        tempDeletedRows.Add(rowGifts);
                    }
                    else if (DetailNumber == 1)
                    {
                        TotalGiftBaseAmount += BaseAmount;
                        tempDeletedRows.Add(rowGifts);

                        // do not print if less than minimum amount
                        if ((TotalGiftBaseAmount < AMinimumAmount)
                            && (TotalGiftBaseAmount > (AMinimumAmount * (-1))))
                        {
                            // if total of gift (can be split gift) is less than minimum amount then mark all gift details to be removed
                            foreach (DataRow tempDataRow in tempDeletedRows)
                            {
                                deletedRows.Add(tempDataRow);
                            }
                        }

                        // now reset total amount and empty temporary table
                        TotalGiftBaseAmount = 0;
                        tempDeletedRows.Clear();
                    }
                }

                // now remove those rows from the original list
                foreach (DataRow dataRow in deletedRows)
                {
                    ADonations.Rows.Remove(dataRow);
                }
            }

            foreach (DataRow rowGifts in ADonations.Rows)
            {
                DateTime dateEntered = Convert.ToDateTime(rowGifts["DateEntered"]);
                decimal amount = Convert.ToDecimal(rowGifts["TransactionAmount"]);
                decimal taxDeductibleAmount = 0;
                decimal nonDeductibleAmount = 0;
                string currency = rowGifts["Currency"].ToString();
                string commentOne = rowGifts["CommentOne"].ToString();
                string commentOneType = rowGifts["CommentOneType"].ToString();
                string commentTwo = rowGifts["CommentTwo"].ToString();
                string commentTwoType = rowGifts["CommentTwoType"].ToString();
                string commentThree = rowGifts["CommentThree"].ToString();
                string commentThreeType = rowGifts["CommentThreeType"].ToString();
                string accountDesc = rowGifts["AccountDesc"].ToString();
                string costcentreDesc = rowGifts["CostCentreDesc"].ToString();
                string fieldName = rowGifts["FieldName"].ToString();
                string recipientName = rowGifts["RecipientName"].ToString();
                string gifttype = rowGifts["GiftType"].ToString();
                RowTemplate = OrigRowTemplate;

                sum += Convert.ToDecimal(rowGifts["AmountInBaseCurrency"]);

                if (TaxDeductiblePercentageEnabled)
                {
                    taxDeductibleAmount = Convert.ToDecimal(rowGifts["TaxDeductibleAmount"]);
                    nonDeductibleAmount = Convert.ToDecimal(rowGifts["NonDeductibleAmount"]);
                    sumTaxDeduct += Convert.ToDecimal(rowGifts["TaxDeductibleAmountBase"]);
                    sumNonDeduct += Convert.ToDecimal(rowGifts["NonDeductibleAmountBase"]);
                }

                /* can we sum up donations on the same date, or do we need to print each detail with the account description? */

                // if we are printing every single gift detail
                if (RowTemplate.Contains("#ACCOUNTDESC") || RowTemplate.Contains("#COSTCENTREDESC")
                    || RowTemplate.Contains("#FIELDNAME") || RowTemplate.Contains("#RECIPIENTNAME")
                    || RowTemplate.Contains("#COMMENTONE") || RowTemplate.Contains("#COMMENTTWO") || RowTemplate.Contains("#COMMENTTHREE"))
                {
                    if (gifttype == MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND)
                    {
                        RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT);
                    }
                    else if (gifttype == MFinanceConstants.GIFT_TYPE_GIFT)
                    {
                        RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND);
                    }

                    GetUnitLabels(currency, ref MajorUnitSingular, ref MajorUnitPlural, ref MinorUnitSingular, ref MinorUnitPlural);

                    rowTexts += RowTemplate.
                                Replace("#DONATIONDATE", dateEntered.ToString("dd.MM.yyyy")).
                                Replace("#AMOUNTCURRENCY", currency).
                                Replace("#AMOUNTINWORDS",
                        NumberToWords.AmountToWords(amount, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                                Replace("#AMOUNT", StringHelper.FormatUsingCurrencyCode(amount, currency)).
                                Replace("#TAXDEDUCTAMOUNTINWORDS",
                        NumberToWords.AmountToWords(taxDeductibleAmount, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                                Replace("#TAXDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(taxDeductibleAmount, currency)).
                                Replace("#TAXNONDEDUCTAMNTINWORDS",
                        NumberToWords.AmountToWords(nonDeductibleAmount, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                                Replace("#TAXNONDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(nonDeductibleAmount, currency)).
                                Replace("#COMMENTONE", commentOne).
                                Replace("#COMMENTTWO", commentTwo).
                                Replace("#COMMENTTHREE", commentThree).
                                Replace("#ACCOUNTDESC", accountDesc).
                                Replace("#COSTCENTREDESC", costcentreDesc).
                                Replace("#FIELDNAME", fieldName).
                                Replace("#RECIPIENTNAME", recipientName);
                }
                // if we are summing up donations on the same date
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

                        GetUnitLabels(prevCurrency, ref MajorUnitSingular, ref MajorUnitPlural, ref MinorUnitSingular, ref MinorUnitPlural);

                        rowTexts += RowTemplate.
                                    Replace("#DONATIONDATE", prevDateEntered.ToString("dd.MM.yyyy")).
                                    Replace("#AMOUNTCURRENCY", prevCurrency).
                                    Replace("#AMOUNTINWORDS",
                            NumberToWords.AmountToWords(prevAmount, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                                    Replace("#AMOUNT", StringHelper.FormatUsingCurrencyCode(prevAmount, prevCurrency)).
                                    Replace("#TAXDEDUCTAMOUNTINWORDS",
                            NumberToWords.AmountToWords(prevAmountTaxDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular,
                                MinorUnitPlural)).
                                    Replace("#TAXDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(prevAmountTaxDeduct, prevCurrency)).
                                    Replace("#TAXNONDEDUCTAMNTINWORDS",
                            NumberToWords.AmountToWords(prevAmountNonDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular,
                                MinorUnitPlural)).
                                    Replace("#TAXNONDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(prevAmountNonDeduct, prevCurrency));
                        prevAmount = amount;

                        if (TaxDeductiblePercentageEnabled)
                        {
                            prevAmountTaxDeduct = taxDeductibleAmount;
                            prevAmountNonDeduct = nonDeductibleAmount;
                        }
                    }
                    else
                    {
                        prevAmount += amount;

                        if (TaxDeductiblePercentageEnabled)
                        {
                            prevAmountTaxDeduct += taxDeductibleAmount;
                            prevAmountNonDeduct += nonDeductibleAmount;
                        }
                    }

                    prevCurrency = currency;
                    prevDateEntered = dateEntered;
                    prevgifttype = gifttype;
                }
            }

            // only used when we are summing up donations on the same date
            if (prevDateEntered != DateTime.MaxValue)
            {
                RowTemplate = OrigRowTemplate;

                if (prevgifttype == MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND)
                {
                    RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT);
                }
                else if (prevgifttype == MFinanceConstants.GIFT_TYPE_GIFT)
                {
                    RowTemplate = TPrinterHtml.RemoveDivWithClass(RowTemplate, MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND);
                }

                GetUnitLabels(prevCurrency, ref MajorUnitSingular, ref MajorUnitPlural, ref MinorUnitSingular, ref MinorUnitPlural);

                rowTexts += RowTemplate.
                            Replace("#DONATIONDATE", prevDateEntered.ToString("dd.MM.yyyy")).
                            Replace("#AMOUNTCURRENCY", prevCurrency).
                            Replace("#AMOUNTINWORDS",
                    NumberToWords.AmountToWords(prevAmount, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                            Replace("#AMOUNT", StringHelper.FormatUsingCurrencyCode(prevAmount, prevCurrency)).
                            Replace("#TAXDEDUCTAMOUNTINWORDS",
                    NumberToWords.AmountToWords(prevAmountTaxDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                            Replace("#TAXDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(prevAmountTaxDeduct, prevCurrency)).
                            Replace("#TAXNONDEDUCTAMNTINWORDS",
                    NumberToWords.AmountToWords(prevAmountNonDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                            Replace("#TAXNONDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(prevAmountNonDeduct, prevCurrency));
                prevAmount = 0.0M;

                if (TaxDeductiblePercentageEnabled)
                {
                    prevAmountTaxDeduct = 0.0M;
                    prevAmountNonDeduct = 0.0M;
                }
            }

            msg = msg.Replace("#OVERALLAMOUNTCURRENCY", ABaseCurrency).
                  Replace("#OVERALLAMOUNT", StringHelper.FormatUsingCurrencyCode(sum, ABaseCurrency)).
                  Replace("#OVERALLTAXDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(sumTaxDeduct, ABaseCurrency)).
                  Replace("#OVERALLTAXNONDEDUCTAMOUNT", StringHelper.FormatUsingCurrencyCode(sumNonDeduct, ABaseCurrency));

            if ((ADonations.Rows.Count == 1) && msg.Contains("#DONATIONDATE"))
            {
                // this is a receipt for just one gift
                msg = msg.Replace("#DONATIONDATE", Convert.ToDateTime(ADonations.Rows[0]["DateEntered"]).ToString("dd.MM.yyyy"));
            }

            GetUnitLabels(ABaseCurrency, ref MajorUnitSingular, ref MajorUnitPlural, ref MinorUnitSingular, ref MinorUnitPlural);

            msg =
                msg.Replace("#TOTALAMOUNTINWORDS",
                    NumberToWords.AmountToWords(sum, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                Replace("#OVERALLTAXDEDUCTAMNTINWORDS",
                    NumberToWords.AmountToWords(sumTaxDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                Replace("#OVERALLTAXNONDEDUCTAMNTINWORDS",
                    NumberToWords.AmountToWords(sumNonDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural));

            return msg.Replace("#ROWTEMPLATE", rowTexts);
        }

        private static void GetUnitLabels(string ACurrency,
            ref string AMajorUnitSingular,
            ref string AMajorUnitPlural,
            ref string AMinorUnitSingular,
            ref string AMinorUnitPlural)
        {
            ACurrencyLanguageRow CurrencyLanguage = TFinanceServerLookups.GetCurrencyLanguage(ACurrency);

            if (CurrencyLanguage != null)
            {
                if (!CurrencyLanguage.IsUnitLabelSingularNull())
                {
                    AMajorUnitSingular = CurrencyLanguage.UnitLabelSingular;
                }
                else
                {
                    AMajorUnitSingular = string.Empty;
                }

                if (!CurrencyLanguage.IsUnitLabelPluralNull())
                {
                    AMajorUnitPlural = CurrencyLanguage.UnitLabelPlural;
                }
                else
                {
                    AMajorUnitPlural = string.Empty;
                }

                if (!CurrencyLanguage.IsDecimalLabelSingularNull())
                {
                    AMinorUnitSingular = CurrencyLanguage.DecimalLabelSingular;
                }
                else
                {
                    AMinorUnitSingular = string.Empty;
                }

                if (!CurrencyLanguage.IsDecimalLabelPluralNull())
                {
                    AMinorUnitPlural = CurrencyLanguage.DecimalLabelPlural;
                }
                else
                {
                    AMinorUnitPlural = string.Empty;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetUnreceiptedGifts(Int32 ALedgerNumber)
        {
            DataTable GiftsTbl = null;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    try
                    {
                        String SqlQuery = "SELECT DISTINCT " +
                                          "a_receipt_number_i AS ReceiptNumber," +
                                          "a_date_entered_d AS DateEntered," +
                                          "p_partner_short_name_c AS Donor," +
                                          "PUB_a_gift.a_batch_number_i AS BatchNumber," +
                                          "PUB_a_gift.a_gift_transaction_number_i AS TransactionNumber," +
                                          "a_reference_c AS Reference " +
                                          "FROM PUB_a_gift LEFT JOIN PUB_p_partner on PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n " +
                                          "WHERE PUB_a_gift.a_ledger_number_i=" + ALedgerNumber.ToString() +
                                          " AND a_receipt_printed_l=FALSE AND p_receipt_each_gift_l=TRUE " +
                                          "ORDER BY BatchNumber";

                        GiftsTbl = DBAccess.GDBAccessObj.SelectDT(SqlQuery, "UnreceiptedGiftsTbl", Transaction);
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });

            return GiftsTbl;
        }

        /// <summary>
        /// Create a single gift receipt
        /// </summary>
        /// <param name="ADonorShortName"></param>
        /// <param name="ADonorKey"></param>
        /// <param name="ADonorClass"></param>
        /// <param name="AGiftCurrency"></param>
        /// <param name="ALocalCountryCode">If the addressee's country is the same as this, it won't be printed on the address label.</param>
        /// <param name="AGiftsThisDonor"></param>
        /// <param name="AHTMLTemplateFilename"></param>
        /// <param name="ATransaction">This can be read-only - nothing is written to the DB.</param>
        /// <returns>Complete (simple) HTML file</returns>
        [NoRemoting]
        public static string FormatHtmlReceipt(
            String ADonorShortName,
            Int64 ADonorKey,
            TPartnerClass ADonorClass,
            String AGiftCurrency,
            string ALocalCountryCode,
            AGiftTable AGiftsThisDonor,
            string AHTMLTemplateFilename,
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
            else
            {
                FormValues["AdresseeFamilyName"].Add(ADonorShortName);
            }

            FormValues["DateToday"].Add(DateTime.Now.ToString("dd MMMM yyyy"));

            // Donor Adress:
            PLocationTable Location;
            string CountryName;

            if (TAddressTools.GetBestAddress(ADonorKey, out Location, out CountryName, ATransaction))
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
                String DateEntered = GiftRow.DateEntered.ToString("dd MMM yyyy");
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
                    FormValues["GiftAmount"].Add(StringHelper.FormatUsingCurrencyCode(DetailRow.GiftTransactionAmount, AGiftCurrency));
                    FormValues["GiftCurrency"].Add(AGiftCurrency);
                    FormValues["MotivationDetail"].Add(DetailRow.MotivationDetailCode);
                    GiftTotal += DetailRow.GiftTransactionAmount;

                    if (DetailRow.TaxDeductible)
                    {
                        FormValues["GiftTxd"].Add("Y");
                        TxdTotal += DetailRow.GiftTransactionAmount;
                    }
                    else
                    {
                        FormValues["GiftTxd"].Add(" ");
                        NonTxdTotal += DetailRow.GiftTransactionAmount;
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

            FormValues["GiftTotalAmount"].Add(StringHelper.FormatUsingCurrencyCode(GiftTotal, AGiftCurrency));
            FormValues["GiftTotalCurrency"].Add(AGiftCurrency);
            FormValues["TxdTotal"].Add(StringHelper.FormatUsingCurrencyCode(TxdTotal, AGiftCurrency));
            FormValues["NonTxdTotal"].Add(StringHelper.FormatUsingCurrencyCode(NonTxdTotal, AGiftCurrency));

            return TFormLettersTools.PrintSimpleHTMLLetter(AHTMLTemplateFilename, FormValues);
        }

        /// <param name="AGiftCurrency"></param>
        /// <param name="ADonorShortName"></param>
        /// <param name="ADonorKey"></param>
        /// <param name="ADonorClass"></param>
        /// <param name="GiftsThisDonor"></param>
        /// <param name="AHTMLTemplateFilename"></param>
        /// <summary></summary>
        /// <returns>A Receipt formatted with HTML</returns>
        [RequireModulePermission("FINANCE-1")]
        public static string PrintGiftReceipt(
            String AGiftCurrency,
            String ADonorShortName,
            Int64 ADonorKey,
            TPartnerClass ADonorClass,
            AGiftTable GiftsThisDonor,
            string AHTMLTemplateFilename
            )
        {
            string HtmlDoc = string.Empty;

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    ref Transaction,
                    delegate
                    {
                        string LocalCountryCode = TAddressTools.GetCountryCodeFromSiteLedger(Transaction);
                        HtmlDoc = FormatHtmlReceipt(
                            ADonorShortName,
                            ADonorKey,
                            ADonorClass,
                            AGiftCurrency,
                            LocalCountryCode,
                            GiftsThisDonor,
                            AHTMLTemplateFilename,
                            Transaction);
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGiftTbl">Custom table from GetUnreceiptedGifts, above</param>
        /// <param name="AHTMLTemplateFilename"></param>
        /// <returns>One or more HTML documents in a single string</returns>
        [RequireModulePermission("FINANCE-1")]
        public static string PrintReceipts(int ALedgerNumber, DataTable AGiftTbl, string AHTMLTemplateFilename)
        {
            string HtmlDoc = string.Empty;

            SortedList <Int64, AGiftTable>GiftsPerDonor = new SortedList <Int64, AGiftTable>();
            SortedList <Int64, TempDonorInfo>DonorInfo = new SortedList <Int64, TempDonorInfo>();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    try
                    {
                        string LocalCountryCode = TAddressTools.GetCountryCodeFromSiteLedger(
                            Transaction);

                        foreach (DataRow Row in AGiftTbl.Rows)
                        {
                            String SqlQuery = "SELECT DISTINCT " +
                                              "a_date_entered_d AS DateEntered," +
                                              "p_partner_short_name_c AS Donor," +
                                              "p_donor_key_n AS DonorKey," +
                                              "p_partner_class_c AS DonorClass," +
                                              "a_reference_c AS Reference, " +
                                              "a_currency_code_c AS GiftCurrency " +
                                              "FROM PUB_a_gift LEFT JOIN PUB_p_partner on PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n "
                                              +
                                              "LEFT JOIN PUB_a_gift_batch ON PUB_a_gift.a_ledger_number_i = PUB_a_gift_batch.a_ledger_number_i AND PUB_a_gift.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i "
                                              +
                                              "WHERE PUB_a_gift.a_ledger_number_i=" + ALedgerNumber +
                                              " AND PUB_a_gift.a_batch_number_i=" + Row["BatchNumber"] +
                                              " AND PUB_a_gift.a_gift_transaction_number_i=" + Row["TransactionNumber"];

                            DataRow TempRow = DBAccess.GDBAccessObj.SelectDT(SqlQuery, "UnreceiptedGiftsTbl", Transaction).Rows[0];

                            Int64 DonorKey = Convert.ToInt64(TempRow["DonorKey"]);
                            //
                            // I need to merge any rows that have the same donor.
                            //

                            if (!GiftsPerDonor.ContainsKey(DonorKey))
                            {
                                GiftsPerDonor.Add(DonorKey, new AGiftTable());
                                DonorInfo.Add(DonorKey, new TempDonorInfo());
                            }

                            TempDonorInfo DonorRow = DonorInfo[DonorKey];
                            DonorRow.DonorShortName = TempRow["Donor"].ToString();
                            DonorRow.DonorClass = SharedTypes.PartnerClassStringToEnum(TempRow["DonorClass"].ToString());
                            DonorRow.GiftCurrency = TempRow["GiftCurrency"].ToString();
                            DonorRow.DateEntered = Convert.ToDateTime(TempRow["DateEntered"]);

                            AGiftRow GiftRow = GiftsPerDonor[DonorKey].NewRowTyped();
                            GiftRow.LedgerNumber = ALedgerNumber;
                            GiftRow.BatchNumber = Convert.ToInt32(Row["BatchNumber"]);
                            GiftRow.GiftTransactionNumber = Convert.ToInt32(Row["TransactionNumber"]);
                            GiftRow.Reference = TempRow["Reference"].ToString();
                            GiftRow.DateEntered = Convert.ToDateTime(TempRow["DateEntered"]);
                            GiftsPerDonor[DonorKey].Rows.Add(GiftRow);
                        } // foreach Row

                        foreach (Int64 DonorKey in GiftsPerDonor.Keys)
                        {
                            TempDonorInfo DonorRow = DonorInfo[DonorKey];
                            string PageHtml = FormatHtmlReceipt(
                                DonorRow.DonorShortName,
                                DonorKey,
                                DonorRow.DonorClass,
                                DonorRow.GiftCurrency,
                                LocalCountryCode,
                                GiftsPerDonor[DonorKey],
                                AHTMLTemplateFilename,
                                Transaction);

                            TFormLettersTools.AttachNextPage(ref HtmlDoc, PageHtml);
                        } // foreach DonorKey

                        TFormLettersTools.CloseDocument(ref HtmlDoc);
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });

            return HtmlDoc;
        }

        /// <summary>Mark a gift as receipted in the AGift table.</summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ATransactionNumber"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void MarkReceiptsPrinted(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 ATransactionNumber)
        {
            AGiftTable Tbl = new AGiftTable();

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        Tbl.Merge(AGiftAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, ATransactionNumber, Transaction));

                        foreach (AGiftRow Row in Tbl.Rows)
                        {
                            Row.ReceiptPrinted = true;
                        }

                        AGiftAccess.SubmitChanges(Tbl, Transaction);

                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });
        }

        /// <summary>Mark selected gifts as receipted in the AGift table.</summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGiftTbl">Custom DataTable from GetUnreceiptedGifts, above.
        /// For this method, only {bool}Selected, LedgerNumber, BatchNumber and TransactionNumber fields are needed.</param>
        /// <returns>True if successful</returns>
        [RequireModulePermission("FINANCE-1")]
        public static void MarkReceiptsPrinted(int ALedgerNumber, DataTable AGiftTbl)
        {
            AGiftTable Tbl = new AGiftTable();

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        foreach (DataRow Row in AGiftTbl.Rows)
                        {
                            Tbl.Merge(AGiftAccess.LoadByPrimaryKey(
                                    ALedgerNumber,
                                    Convert.ToInt32(Row["BatchNumber"]),
                                    Convert.ToInt32(Row["TransactionNumber"]),
                                    Transaction));
                        }

                        foreach (AGiftRow Row in Tbl.Rows)
                        {
                            Row.ReceiptPrinted = true;
                        }

                        AGiftAccess.SubmitChanges(Tbl, Transaction);

                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Int32 GetLastReceiptNumber(Int32 ALedgerNumber)
        {
            ALedgerTable LedgerTbl = null;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    try
                    {
                        LedgerTbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });

            if ((LedgerTbl != null) && (LedgerTbl.Rows.Count > 0))
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
            ALedgerTable LedgerTbl = null;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        LedgerTbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

                        if (LedgerTbl.Rows.Count > 0)
                        {
                            LedgerTbl[0].LastHeaderRNumber = AReceiptNumber;

                            ALedgerAccess.SubmitChanges(LedgerTbl, Transaction);

                            SubmissionOK = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });
        }
    }
}
