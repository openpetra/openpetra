//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2024 by OM International
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
using System.Text;
using System.Diagnostics;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Printing;
using Ict.Common.Remoting.Shared;

using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
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
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    ///<summary>
    /// This connector allows creating the gift receipts
    ///</summary>
    public class TReceiptingWebConnector
    {
        private const string PARTNERTYPE_THANKYOU_NO_RECEIPT = "THANKYOU_NO_RECEIPT";

        /// <summary>
        /// create the annual gift receipts for all donors in the given year;
        /// returns the PDF file containing all receipts
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool CreateAnnualGiftReceipts(Int32 ALedgerNumber,
            string AFrequency,
            DateTime AStartDate,
            DateTime AEndDate,
            string AHTMLTemplate,
            byte[] ALogoImage,
            string ALogoFilename,
            byte[] ASignatureImage,
            string ASignatureFilename,
            string ALanguage,
            string AEmailSubject,
            string AEmailBody,
            string AEmailFrom,
            string AEmailFromName,
            string AEmailFilename,
            out string APDFReceipt,
            out string AHTMLReceipt,
            out TVerificationResultCollection AVerification,
            bool ADeceasedFirst = false,
            string AExtract = null,
            Int64 ADonorKey = 0,
            string AAction = "all",
            bool AOnlyTest = true)
        {
            string ResultDocument = string.Empty;
            APDFReceipt = string.Empty;
            AHTMLReceipt = String.Empty;
            AVerification = new TVerificationResultCollection();

            Catalog.Init(ALanguage, ALanguage);

            // get BaseCurrency
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ALedgerTable LedgerTable = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                ALedgerNumber,
                out typeofTable);
            string BaseCurrency = LedgerTable[0].BaseCurrency;

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("AnnualGiftReceipts");

            TSystemDefaults SystemDefaults = new TSystemDefaults(db);
            string EmailPublicationCode = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_PUBLICATION_CODE, String.Empty);

            if (AEmailSubject != String.Empty)
            {
                SystemDefaults.SetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_SUBJECT, AEmailSubject, db);
                SystemDefaults.SetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_BODY, AEmailBody, db);
                SystemDefaults.SetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_FROM, AEmailFrom, db);
                SystemDefaults.SetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_FROMNAME, AEmailFromName, db);
                SystemDefaults.SetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_FILENAME, AEmailFilename, db);
            }

            if (EmailPublicationCode == String.Empty && (AAction == "email"))
            {
                AVerification.Add(new TVerificationResult(
                        Catalog.GetString("Sending Email"),
                        Catalog.GetString("Please specify the Email Publication Code in the ledger settings"),
                        "Server problems",
                        TResultSeverity.Resv_Critical,
                        new System.Guid()));
                TLogging.Log("Please specify the Email Publication Code in the ledger settings");
                return false;
            }

            if (AAction == "all")
            {
                AOnlyTest = true;
            }

            TVerificationResultCollection LocalVerification = new TVerificationResultCollection();

            db.ReadTransaction(
                ref Transaction,
                delegate
                {
                    try
                    {
                        if (AHTMLTemplate == String.Empty)
                        {
                            string TmpFilename;
                            byte[] data = Convert.FromBase64String(LoadFileTemplate("AnnualReceiptHTML", Transaction, out TmpFilename));
                            AHTMLTemplate = Encoding.UTF8.GetString(data);
                        }

                        if (ALogoFilename == String.Empty)
                        {
                            ALogoImage = Convert.FromBase64String(LoadFileTemplate("AnnualReceiptLOGO", Transaction, out ALogoFilename));
                        }

                        if (ASignatureFilename == String.Empty)
                        {
                            ASignatureImage = Convert.FromBase64String(LoadFileTemplate("AnnualReceiptSIGN", Transaction, out ASignatureFilename));
                        }

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
                            donorkeys.Columns.Add(new DataColumn("Status"));
                            donorkeys.Columns.Add(new DataColumn("OnlyThankYouNoReceipt"));
                            DataRow SingleRow = donorkeys.NewRow();
                            SingleRow[0] = ADonorKey;
                            SingleRow[1] = ShortName;
                            SingleRow[2] = String.Empty;
                            SingleRow[3] = (DoNotSendReceiptButThankyouLetter(ADonorKey)?PARTNERTYPE_THANKYOU_NO_RECEIPT:String.Empty);

                            donorkeys.Rows.Add(SingleRow);
                        }
                        else
                        {
                            SortedList <string, string>Defines = new SortedList <string, string>();
                            int EmailPublicationCodePos = -1;
                            int CountParameters = 6;

                            if (!string.IsNullOrEmpty(AExtract))
                            {
                                Defines.Add("BYEXTRACT", string.Empty);
                                CountParameters += 1;
                            }

                            if (EmailPublicationCode != String.Empty)
                            {
                                if (AAction == "print")
                                {
                                    Defines.Add("VIAPRINT", string.Empty);
                                    EmailPublicationCodePos = CountParameters;
                                    CountParameters += 1;
                                }
                                else if (AAction == "email")
                                {
                                    Defines.Add("VIAEMAIL", string.Empty);
                                    EmailPublicationCodePos = CountParameters;
                                    CountParameters += 1;
                                }
                            }

                            // first get all donors in the given date range
                            SqlStmt = TDataBase.ReadSqlFile("Gift.ReceiptPrinting.GetDonors.sql", Defines);

                            OdbcParameter[] parameters = new OdbcParameter[CountParameters];
                            parameters[0] = new OdbcParameter("PartnerTypeThankYou", OdbcType.VarChar);
                            parameters[0].Value = PARTNERTYPE_THANKYOU_NO_RECEIPT;
                            parameters[1] = new OdbcParameter("LedgerNumber", OdbcType.Int);
                            parameters[1].Value = ALedgerNumber;
                            parameters[2] = new OdbcParameter("StartDate", OdbcType.Date);
                            parameters[2].Value = AStartDate;
                            parameters[3] = new OdbcParameter("EndDate", OdbcType.Date);
                            parameters[3].Value = AEndDate;
                            parameters[4] = new OdbcParameter("IgnoreFrequency", OdbcType.Bit);

                            if (AFrequency == "")
                            {
                                parameters[4].Value = true;
                            }
                            else
                            {
                                parameters[4].Value = false;
                            }

                            parameters[5] = new OdbcParameter("Frequency", OdbcType.VarChar);
                            parameters[5].Value = AFrequency.ToUpper();

                            if (!string.IsNullOrEmpty(AExtract))
                            {
                                parameters[6] = new OdbcParameter("Extract", OdbcType.VarChar);
                                parameters[6].Value = AExtract;
                            }

                            if (EmailPublicationCodePos > 0)
                            {
                                parameters[EmailPublicationCodePos] = new OdbcParameter("EmailPublicationCode", OdbcType.VarChar);
                                parameters[EmailPublicationCodePos].Value = EmailPublicationCode;
                            }

                            donorkeys = db.SelectDT(SqlStmt, "DonorKeys", Transaction, parameters);

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

                            bool OnlyThankYouNoReceipt = (GetStringOrEmpty(donorrow[3]) == PARTNERTYPE_THANKYOU_NO_RECEIPT);

                            OdbcParameter[] parameters = new OdbcParameter[4];
                            parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
                            parameters[0].Value = ALedgerNumber;
                            parameters[1] = new OdbcParameter("StartDate", OdbcType.Date);
                            parameters[1].Value = AStartDate;
                            parameters[2] = new OdbcParameter("EndDate", OdbcType.Date);
                            parameters[2].Value = AEndDate;
                            parameters[3] = new OdbcParameter("DonorKey", OdbcType.BigInt);
                            parameters[3].Value = donorKey;

                            DataTable donations = db.SelectDT(SqlStmt, "Donations", Transaction, parameters);

                            if (donations.Rows.Count > 0)
                            {
                                string letter =
                                    FormatLetter(donorKey, donorName, AOnlyTest, OnlyThankYouNoReceipt, donations, BaseCurrency, AHTMLTemplate, LocalCountryCode, -1, true, Transaction);

                                // attach the current receipt to the pdf that contains all receipts
                                TFormLettersTools.AttachNextPage(ref ResultDocument, letter);

                                bool ReceiptSuccess = false;

                                if (AAction == "email")
                                {
                                    // print single receipt to separate temporary pdf file
                                    string PDFFile = TFileHelper.GetTempFileName(
                                        AEmailFilename.Replace(".pdf", "") + "_" + donorKey,
                                        ".pdf");

                                    if (PrintToPDF(PDFFile, letter,
                                        ALogoImage, ALogoFilename, ASignatureImage, ASignatureFilename))
                                    {
                                        TVerificationResultCollection EmailVerification = null;

                                        // get the valid Email Address of the donor
                                        string ReceipientEmail = TMailing.GetBestEmailAddress(donorKey);

                                        if (ReceipientEmail == string.Empty)
                                        {
                                            throw new Exception("cannot find E-Mail address for Donor " + donorKey.ToString());
                                        }

                                        string EmailDonorName = Ict.Petra.Server.MPartner.Common.Calculations.FormatShortName(donorName,
                                            eShortNameFormat.eReverseWithoutTitle);

                                        // send email with pdf to donor
                                        if (!SendEmail(ReceipientEmail, EmailDonorName,
                                            AEmailSubject, AEmailBody.Replace("{{donorName}}", EmailDonorName),
                                            AEmailFrom, AEmailFromName,
                                            AEmailFrom,
                                            AEmailFilename,
                                            PDFFile,
                                            AOnlyTest,
                                            out EmailVerification))
                                        {
                                            TLogging.Log("Cannot send receipt email to " + EmailDonorName + " " + donorKey.ToString());
                                            LocalVerification.AddCollection(EmailVerification);
                                        }
                                        else
                                        {
                                            ReceiptSuccess = true;
                                        }

                                        File.Delete(PDFFile);
                                    }
                                }
                                else if (AAction == "print")
                                {
                                    ReceiptSuccess = true;
                                }

                                if (ReceiptSuccess && !AOnlyTest)
                                {
                                    // TODO store information that the receipt has been sent
                                    // so that it will not be sent again
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

            AVerification.AddCollection(LocalVerification);

            if (ResultDocument.Length > 0)
            {
                string PDFFile = TFileHelper.GetTempFileName("receipts",".pdf");

                if (PrintToPDF(PDFFile, ResultDocument,
                    ALogoImage, ALogoFilename, ASignatureImage, ASignatureFilename))
                {
                    byte[] data = System.IO.File.ReadAllBytes(PDFFile);
                    APDFReceipt = Convert.ToBase64String(data);
                    System.IO.File.Delete(PDFFile);
                }

                AHTMLReceipt = THttpBinarySerializer.SerializeToBase64(ResultDocument);
            }

            return ResultDocument.Length > 0 && !AVerification.HasCriticalErrors;
        }

        private static bool PrintToPDF(string AOutputPDFFilename,
            string strHTMLData,
            byte[] ALogoImage,
            string ALogoFilename,
            byte[] ASignatureImage,
            string ASignatureFilename)
        {
            // Store the images to the temp directory of this instance
            string tempFileNameLogo = string.Empty;
            string tempFileNameSignature = string.Empty;

            if (ALogoFilename.Length > 0)
            {
                tempFileNameLogo = TFileHelper.GetTempFileName(
                    "logo",
                    Path.GetExtension(ALogoFilename));
                strHTMLData = strHTMLData.Replace(ALogoFilename, tempFileNameLogo);

                File.WriteAllBytes(tempFileNameLogo, ALogoImage);
            }

            if (ASignatureFilename.Length > 0)
            {
                tempFileNameSignature = TFileHelper.GetTempFileName(
                    "signature",
                    Path.GetExtension(ASignatureFilename));

                strHTMLData = strHTMLData.Replace(ASignatureFilename, tempFileNameSignature);
                File.WriteAllBytes(tempFileNameSignature, ASignatureImage);
            }

            string HTMLFile = TFileHelper.GetTempFileName(
                "receipts",
                ".html");

            strHTMLData = Html2Pdf.StripLastPageBreak(strHTMLData);

            using (StreamWriter sw = new StreamWriter(HTMLFile))
            {
                sw.Write(strHTMLData);
                sw.Close();
            }

            Process process = new Process();
            process.StartInfo.FileName = Html2Pdf.GetWkHTMLToPDFPath();
            process.StartInfo.Arguments = "--disable-local-file-access --allow " + Path.GetDirectoryName(HTMLFile) + " " + HTMLFile + " " + AOutputPDFFilename;
            process.Start();
            process.WaitForExit();

            File.Delete(HTMLFile);

            if (tempFileNameLogo.Length > 0)
            {
                File.Delete(tempFileNameLogo);
            }

            if (tempFileNameSignature.Length > 0)
            {
                File.Delete(tempFileNameSignature);
            }

            return true;
        }

        private static string GetStringOrEmpty(object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return "";
            }

            return obj.ToString();
        }

        private static bool SendEmail(
            string AEmailRecipient, string AEmailRecipientName,
            string AEmailSubject, string AEmailBody,
            string AEmailFrom, string AEmailFromName,
            string AEmailBCC,
            string ADisplayFilename,
            string AFilenamePDFReceipt,
            bool AOnlyTest,
            out TVerificationResultCollection AVerification)
        {
            AVerification = new TVerificationResultCollection();

            if (AOnlyTest)
            {
                // do not send to the donor
                AEmailRecipient = AEmailFrom.Replace("@", "+testdonor@");
                // BCC to test address
                AEmailFrom = AEmailFrom.Replace("@", "+test@");
            }

            TSmtpSender EmailSender = new TSmtpSender();

            try
            {
                if (TAppSettingsManager.GetValue("SmtpHost").EndsWith(TSmtpSender.SMTP_HOST_DEFAULT))
                {
                    TLogging.Log("There is no configuration for SmtpHost.");
                    return false;
                }

                try
                {
                    EmailSender.SetSender("no-reply@" + TAppSettingsManager.GetValue("Server.EmailDomain"), AEmailFromName);
                    EmailSender.ReplyTo = AEmailFrom;
                    EmailSender.BccEverythingTo = AEmailFrom;
                    EmailSender.AddAttachment(ADisplayFilename, AFilenamePDFReceipt);
                }
                catch (Exception e)
                {
                    TLogging.Log("There is an issue with setting email parameters.");
                    TLogging.Log(e.ToString());
                    return false;
                }

                if (EmailSender.SendEmail(
                        AEmailRecipientName + " <" + AEmailRecipient + ">",
                        AEmailSubject,
                        AEmailBody))
                {
                    return true;
                }

                AVerification.Add(new TVerificationResult(
                        Catalog.GetString("Sending Email"),
                        Catalog.GetString("Problem sending email"),
                        "Server problems",
                        TResultSeverity.Resv_Critical,
                        new System.Guid()));

                return false;
            } // try
            catch (ESmtpSenderInitializeException e)
            {
                AVerification.Add(new TVerificationResult(
                        Catalog.GetString("Sending Email"),
                        e.Message,
                        CommonErrorCodes.ERR_MISSINGEMAILCONFIGURATION,
                        TResultSeverity.Resv_Critical,
                        new System.Guid()));

                return false;
            }
            finally
            {
                if (EmailSender != null)
                {
                    EmailSender.Dispose();
                }
            }
        }

        /// <summary>
        /// Do we have any consent at all for this contact.
        /// This method should be in Ict.Petra.Server.MPartner.Partner.WebConnectors.TDataHistoryWebConnector,
        /// but that would cause a cyclic dependancy.
        /// </summary>
        private static bool UndefinedConsent(Int64 APartnerKey)
        {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get Last known entry");
            List<OdbcParameter> SQLParameter = new List<OdbcParameter>();
            bool HasConsent = false;

            DB.ReadTransaction(ref T, delegate {

                string sql = "SELECT " +
                    "COUNT(*)" +
                    "FROM `p_consent_history` " +
                    "WHERE `p_consent_history`.`p_partner_key_n` = ?";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey } );

                HasConsent = (Convert.ToInt32(DB.ExecuteScalar(sql, T, SQLParameter.ToArray())) > 0);
            });

            return !HasConsent;
        }

        /// <summary>
        /// Does this partner have a given subscription
        /// </summary>
        private static bool HasSubscription(Int64 APartnerKey, String APublicationCode)
        {
            if ((APublicationCode == null) || (APublicationCode == String.Empty))
            {
                return false;
            }

            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("HasSubscription");
            List<OdbcParameter> SQLParameter = new List<OdbcParameter>();
            bool ResultValue = false;

            DB.ReadTransaction(ref T, delegate {

                string sql = "SELECT " +
                    "COUNT(*) " +
                    "FROM `p_subscription` " +
                    "WHERE `p_subscription`.`p_partner_key_n` = ? " +
                    "AND `p_publication_code_c` = ? " +
                    "AND (`p_start_date_d` IS NULL OR `p_start_date_d` <= NOW()) " +
                    "AND (`p_expiry_date_d` IS NULL OR `p_expiry_date_d` <= NOW())";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey } );
                SQLParameter.Add(new OdbcParameter("PublicationCode", OdbcType.VarChar) { Value = APublicationCode } );

                ResultValue = (Convert.ToInt32(DB.ExecuteScalar(sql, T, SQLParameter.ToArray())) > 0);
            });

            return ResultValue;
        }

        /// <summary>
        /// Should this partner receive a donation receipt, or just a thank you letter
        /// </summary>
        private static bool DoNotSendReceiptButThankyouLetter(Int64 APartnerKey)
        {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get Partner Type " + PARTNERTYPE_THANKYOU_NO_RECEIPT);
            List<OdbcParameter> SQLParameter = new List<OdbcParameter>();
            bool ThankYouNoReceipt = false;

            DB.ReadTransaction(ref T, delegate {

                string sql = "SELECT " +
                    "COUNT(*)" +
                    "FROM `p_partner_type` " +
                    "WHERE `p_partner_type`.`p_partner_key_n` = ? " +
                    "AND `p_type_code_c` = '" + PARTNERTYPE_THANKYOU_NO_RECEIPT + "'";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey } );

                ThankYouNoReceipt = (Convert.ToInt32(DB.ExecuteScalar(sql, T, SQLParameter.ToArray())) > 0);
            });

            return ThankYouNoReceipt;
        }

        private static string ReplaceIfSection(string msg, string name, bool condition)
        {
            while (msg.Contains("#if " + name))
            {
                int indexIf = msg.IndexOf("#if " + name);

                int indexEndif = msg.IndexOf("#endif", indexIf);
                if (indexEndif == -1)
                {
                    throw new Exception("missing #endif for " + name);
                }

                if (condition)
                {
                    // just drop the #if and #endif line
                    msg = msg.Substring(0, indexEndif) + msg.Substring(indexEndif + "#endif".Length);
                    msg = msg.Substring(0, indexIf) + msg.Substring(indexIf + ("#if " + name).Length);
                }
                else
                {
                    // drop the whole #if section
                    msg = msg.Substring(0, indexIf) + msg.Substring(indexEndif + "#endif".Length);
                }
            }

            return msg;
        }

        /// <summary>
        /// Format the letter for the donor with all the gifts
        ///
        /// Can also used for a single receipt.
        /// </summary>
        /// <returns>One or more html documents, each in its own body tag, for printing with the HTML printer</returns>
        private static string FormatLetter(Int64 ADonorKey,
            string ADonorName,
            bool AOnlyTest,
            bool AOnlyThankYouNoReceipt,
            DataTable ADonations,
            string ABaseCurrency,
            string AHTMLTemplate,
            string ALedgerCountryCode,
            Decimal AMinimumAmount,
            bool AAlwaysPrintNewDonor,
            TDBTransaction ATransaction)
        {
            // get details of the donor, and best address

            StringHelper myStringHelper = new StringHelper();
            myStringHelper.CurrencyFormatTable = ATransaction.DataBaseObj.SelectDT("SELECT * FROM PUB_a_currency", "a_currency", ATransaction);

            PLocationTable Location;
            string CountryName;

            string MajorUnitSingular = string.Empty;
            string MajorUnitPlural = string.Empty;
            string MinorUnitSingular = string.Empty;
            string MinorUnitPlural = string.Empty;

            if (!TAddressTools.GetBestAddressOnlySendMail(ADonorKey, out Location, out CountryName, ATransaction))
            {
                return "";
            }

            bool TaxDeductiblePercentageEnabled =
                new TSystemDefaults(ATransaction.DataBaseObj).GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

            string msg = AHTMLTemplate;

            // replace sections about consent for a defined purpose
            msg = ReplaceIfSection(msg, "UNDEFINEDCONSENT", UndefinedConsent(ADonorKey));

            msg = ReplaceIfSection(msg, "TEST", AOnlyTest);

            while (msg.Contains("SUBSCRIPTION("))
            {
                int pos = msg.IndexOf("SUBSCRIPTION(") + "SUBSCRIPTION(".Length;
                string PublicationCode = msg.Substring(pos, msg.IndexOf(")", pos) - pos);

                bool hasSubscription = HasSubscription(ADonorKey, PublicationCode);

                msg = ReplaceIfSection(msg, "NOTSUBSCRIPTION(" + PublicationCode + ")", !hasSubscription);
                msg = ReplaceIfSection(msg, "SUBSCRIPTION(" + PublicationCode + ")", hasSubscription);
            }

            // replace sections about thank you letters vs receipts
            msg = ReplaceIfSection(msg, "THANKYOU", AOnlyThankYouNoReceipt);
            msg = ReplaceIfSection(msg, "RECEIPT", !AOnlyThankYouNoReceipt);

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
                string commentTwo = rowGifts["CommentTwo"].ToString();
                string commentThree = rowGifts["CommentThree"].ToString();
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
                                Replace("#AMOUNT", myStringHelper.FormatUsingCurrencyCode(amount, currency)).
                                Replace("#TAXDEDUCTAMOUNTINWORDS",
                        NumberToWords.AmountToWords(taxDeductibleAmount, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                                Replace("#TAXDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(taxDeductibleAmount, currency)).
                                Replace("#TAXNONDEDUCTAMNTINWORDS",
                        NumberToWords.AmountToWords(nonDeductibleAmount, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                                Replace("#TAXNONDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(nonDeductibleAmount, currency)).
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
                                    Replace("#AMOUNT", myStringHelper.FormatUsingCurrencyCode(prevAmount, prevCurrency)).
                                    Replace("#TAXDEDUCTAMOUNTINWORDS",
                            NumberToWords.AmountToWords(prevAmountTaxDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular,
                                MinorUnitPlural)).
                                    Replace("#TAXDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(prevAmountTaxDeduct, prevCurrency)).
                                    Replace("#TAXNONDEDUCTAMNTINWORDS",
                            NumberToWords.AmountToWords(prevAmountNonDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular,
                                MinorUnitPlural)).
                                    Replace("#TAXNONDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(prevAmountNonDeduct, prevCurrency));
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
                            Replace("#AMOUNT", myStringHelper.FormatUsingCurrencyCode(prevAmount, prevCurrency)).
                            Replace("#TAXDEDUCTAMOUNTINWORDS",
                    NumberToWords.AmountToWords(prevAmountTaxDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                            Replace("#TAXDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(prevAmountTaxDeduct, prevCurrency)).
                            Replace("#TAXNONDEDUCTAMNTINWORDS",
                    NumberToWords.AmountToWords(prevAmountNonDeduct, MajorUnitSingular, MajorUnitPlural, MinorUnitSingular, MinorUnitPlural)).
                            Replace("#TAXNONDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(prevAmountNonDeduct, prevCurrency));
                prevAmount = 0.0M;

                if (TaxDeductiblePercentageEnabled)
                {
                    prevAmountTaxDeduct = 0.0M;
                    prevAmountNonDeduct = 0.0M;
                }
            }

            msg = msg.Replace("#OVERALLAMOUNTCURRENCY", ABaseCurrency).
                  Replace("#OVERALLAMOUNT", myStringHelper.FormatUsingCurrencyCode(sum, ABaseCurrency)).
                  Replace("#OVERALLTAXDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(sumTaxDeduct, ABaseCurrency)).
                  Replace("#OVERALLTAXNONDEDUCTAMOUNT", myStringHelper.FormatUsingCurrencyCode(sumNonDeduct, ABaseCurrency));

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
            ACurrencyLanguageRow CurrencyLanguage = TFinanceServerLookupWebConnector.GetCurrencyLanguage(ACurrency);

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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetUnreceiptedGifts");

            db.ReadTransaction(
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
                                          " AND (EXISTS (SELECT 1 FROM PUB_a_gift_detail d LEFT JOIN a_motivation_detail m ON m.a_ledger_number_i = d.a_ledger_number_i AND m.a_motivation_group_code_c = d.a_motivation_group_code_c AND m.a_motivation_detail_code_c = d.a_motivation_detail_code_c "
                                          +
                                          "WHERE d.a_ledger_number_i = PUB_a_gift.a_ledger_number_i " +
                                          "AND d.a_batch_number_i = PUB_a_gift.a_batch_number_i " +
                                          "AND d.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i " +
                                          "AND m.a_receipt_l=TRUE)) " +
                                          "ORDER BY BatchNumber";

                        GiftsTbl = db.SelectDT(SqlQuery, "UnreceiptedGiftsTbl", Transaction);
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
            StringHelper myStringHelper = new StringHelper();
            myStringHelper.CurrencyFormatTable = ATransaction.DataBaseObj.SelectDT("SELECT * FROM PUB_a_currency", "a_currency", ATransaction);

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
                    FormValues["GiftAmount"].Add(myStringHelper.FormatUsingCurrencyCode(DetailRow.GiftTransactionAmount, AGiftCurrency));
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

            FormValues["GiftTotalAmount"].Add(myStringHelper.FormatUsingCurrencyCode(GiftTotal, AGiftCurrency));
            FormValues["GiftTotalCurrency"].Add(AGiftCurrency);
            FormValues["TxdTotal"].Add(myStringHelper.FormatUsingCurrencyCode(TxdTotal, AGiftCurrency));
            FormValues["NonTxdTotal"].Add(myStringHelper.FormatUsingCurrencyCode(NonTxdTotal, AGiftCurrency));

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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("PrintGiftReceipt");

            try
            {
                db.ReadTransaction(
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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("PrintReceipts");

            db.ReadTransaction(
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
                                              "a_receipt_number_i AS ReceiptNumber," +
                                              "a_currency_code_c AS GiftCurrency " +
                                              "FROM PUB_a_gift LEFT JOIN PUB_p_partner on PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n "
                                              +
                                              "LEFT JOIN PUB_a_gift_batch ON PUB_a_gift.a_ledger_number_i = PUB_a_gift_batch.a_ledger_number_i AND PUB_a_gift.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i "
                                              +
                                              "WHERE PUB_a_gift.a_ledger_number_i=" + ALedgerNumber +
                                              " AND PUB_a_gift.a_batch_number_i=" + Row["BatchNumber"] +
                                              " AND PUB_a_gift.a_gift_transaction_number_i=" + Row["TransactionNumber"];

                            DataRow TempRow = db.SelectDT(SqlQuery, "UnreceiptedGiftsTbl", Transaction).Rows[0];

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
                            GiftRow.ReceiptNumber = Convert.ToInt32(TempRow["ReceiptNumber"]);
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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("MarkReceiptsPrinted");
            bool SubmissionOK = false;

            db.WriteTransaction(
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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("MarkReceiptsPrinted");
            bool SubmissionOK = false;

            db.WriteTransaction(
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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetLastReceiptNumber");

            db.ReadTransaction(
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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetLastReceiptNumber");
            bool SubmissionOK = false;

            db.WriteTransaction(
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

        /// Store a default HTML or logo or signature file
        [RequireModulePermission("FINANCE-3")]
        public static bool StoreDefaultFile(string APurpose, string AFileName, byte[] AFileContent,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();
            string FormCode = "AnnualReceipt" + APurpose;
            string FormName = FormCode;
            string FormLanguage = "99";

            if (FormCode.Length > 20)
            {
                AVerificationResult.Add(new TVerificationResult("error", "FormCode is too long", "",
                    "AnnualReceipts.ErrFormCodeTooLong", TResultSeverity.Resv_Critical));
                return false;
            }

            if (AFileName.Length > 100)
            {
                AFileName = AFileName.Substring(AFileName.Length - 100);
            }

            PFormTable Tbl = new PFormTable();

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("StoreDefaultFile");
            bool SubmissionOK = false;

            db.WriteTransaction(
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        Tbl.Merge(PFormAccess.LoadByPrimaryKey(FormCode, FormName, FormLanguage, Transaction));

                        PFormRow FormRow = null;

                        if (Tbl.Rows.Count == 0)
                        {
                            FormRow = Tbl.NewRowTyped(true);
                            FormRow.FormCode = FormCode;
                            FormRow.FormName = FormName;
                            FormRow.FormLanguage = FormLanguage;
                            FormRow.FormTypeCode = "ANNUALRECEIPT";
                            Tbl.Rows.Add(FormRow);
                        }

                        FormRow = Tbl[0];
                        FormRow.TemplateDocument = Convert.ToBase64String(AFileContent);
                        FormRow.FormDescription = AFileName;
                        FormRow.TemplateFileExtension = Path.GetExtension(AFileName);

                        PFormAccess.SubmitChanges(Tbl, Transaction);
                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                    }
                });

                if (!SubmissionOK)
                {
                    AVerificationResult.Add(new TVerificationResult("error", "Some error storing the template", "",
                        "AnnualReceipts.ErrSavingTemplate", TResultSeverity.Resv_Critical));
                }

                return SubmissionOK;
        }

        /// get the filenames of the stored template files for annual gift receipt, and other defaults
        [RequireModulePermission("FINANCE-1")]
        public static bool LoadReceiptDefaults(
            out string AFileNameHTML, out string AFileNameLogo, out string AFileNameSignature,
            out string AEmailSubject, out string AEmailBody, out string AEmailFrom, out string AEmailFromName,
            out string AEmailFilename)
        {
            AFileNameHTML = String.Empty;
            AFileNameLogo = String.Empty;
            AFileNameSignature = String.Empty;

            string FileNameHTML = String.Empty;
            string FileNameLogo = String.Empty;
            string FileNameSignature = String.Empty;

            string FormCode = "AnnualReceipt";
            string FormLanguage = "99";

            PFormTable Tbl = new PFormTable();

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("LoadDefaultFile");

            TSystemDefaults SystemDefaults = new TSystemDefaults(db);
            AEmailSubject = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_SUBJECT, String.Empty);
            AEmailBody = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_BODY, String.Empty);
            AEmailFrom = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_FROM, String.Empty);
            AEmailFromName = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_FROMNAME, String.Empty);
            AEmailFilename = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GIFT_RECEIPT_EMAIL_FILENAME, String.Empty);

            db.ReadTransaction(
                ref Transaction,
                delegate
                {
                    StringCollection Columns = new StringCollection();
                    Columns.Add(PFormTable.GetFormDescriptionDBName());

                    Tbl = PFormAccess.LoadByPrimaryKey(FormCode + "HTML", FormCode + "HTML", FormLanguage, Columns, Transaction);

                    if (Tbl.Rows.Count > 0)
                    {
                        FileNameHTML = Tbl[0].FormDescription;
                    }

                    Tbl = PFormAccess.LoadByPrimaryKey(FormCode + "LOGO", FormCode + "LOGO", FormLanguage, Columns, Transaction);

                    if (Tbl.Rows.Count > 0)
                    {
                        FileNameLogo = Tbl[0].FormDescription;
                    }

                    Tbl = PFormAccess.LoadByPrimaryKey(FormCode + "SIGN", FormCode + "SIGN", FormLanguage, Columns, Transaction);

                    if (Tbl.Rows.Count > 0)
                    {
                        FileNameSignature = Tbl[0].FormDescription;
                    }
                });

            AFileNameHTML = FileNameHTML;
            AFileNameLogo = FileNameLogo;
            AFileNameSignature = FileNameSignature;

            return true;
        }

        private static string LoadFileTemplate(string FormCode, TDBTransaction Transaction, out string AFileName)
        {
            AFileName = String.Empty;
            string FormLanguage = "99";

            StringCollection Columns = new StringCollection();
            Columns.Add(PFormTable.GetTemplateDocumentDBName());
            Columns.Add(PFormTable.GetFormDescriptionDBName());
            PFormTable Tbl = PFormAccess.LoadByPrimaryKey(FormCode, FormCode, FormLanguage, Columns, Transaction);

            if (Tbl.Rows.Count > 0)
            {
                AFileName = Tbl[0].FormDescription;
                return Tbl[0].TemplateDocument;
            }

            return String.Empty;
        }
    }
}
