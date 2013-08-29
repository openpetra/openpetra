//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;

namespace Ict.Petra.Tools.SampleDataConstructor
{
    /// <summary>
    /// tools for generating and posting gift batches with sample data
    /// </summary>
    public class SampleDataGiftBatches
    {
        /// LedgerNumber to be set from outside
        public static int FLedgerNumber = 43;

        private static SortedList <DateTime, List <XmlNode>>GiftsPerDate;

        /// <summary>
        /// load the gift batches from a text file that was generated with Benerator
        /// </summary>
        /// <param name="AInputBeneratorFile"></param>
        public static void LoadBatches(string AInputBeneratorFile)
        {
            GiftsPerDate = SortGiftsByDate(AInputBeneratorFile);
        }

        /// <summary>
        /// generate the gift batches from a text file that was generated with Benerator, for a specific period
        /// </summary>
        public static void CreateGiftBatches(int APeriodNumber)
        {
            GiftBatchTDS MainDS = CreateGiftBatches(GiftsPerDate, APeriodNumber);

            TVerificationResultCollection VerificationResult;

            MainDS.ThrowAwayAfterSubmitChanges = true;
            GiftBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult);

            if (VerificationResult.HasCriticalOrNonCriticalErrors)
            {
                throw new Exception(VerificationResult.BuildVerificationResultString());
            }
        }

        /// <summary>
        /// post all gift batches in the given period, but leave some (or none) unposted
        /// </summary>
        public static bool PostBatches(int AYear, int APeriod, int ALeaveBatchesUnposted = 0)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            AGiftBatchRow GiftBatchTemplateRow = MainDS.AGiftBatch.NewRowTyped(false);

            GiftBatchTemplateRow.LedgerNumber = FLedgerNumber;
            GiftBatchTemplateRow.BatchYear = AYear;
            GiftBatchTemplateRow.BatchPeriod = APeriod;
            GiftBatchTemplateRow.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;
            AGiftBatchAccess.LoadUsingTemplate(MainDS, GiftBatchTemplateRow, null);

            int countUnPosted = MainDS.AGiftBatch.Count;

            List <Int32>GiftBatchesToPost = new List <int>();

            foreach (AGiftBatchRow batch in MainDS.AGiftBatch.Rows)
            {
                if (countUnPosted <= ALeaveBatchesUnposted)
                {
                    break;
                }

                countUnPosted--;

                GiftBatchesToPost.Add(batch.BatchNumber);
            }

            TVerificationResultCollection VerificationResult;

            if (!TGiftTransactionWebConnector.PostGiftBatches(FLedgerNumber, GiftBatchesToPost, out VerificationResult))
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                return false;
            }

            return true;
        }

        private static SortedList <DateTime, List <XmlNode>>SortGiftsByDate(string AInputBeneratorFile)
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AInputBeneratorFile, ",");

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            SortedList <DateTime, List <XmlNode>>GiftsPerDate = new SortedList <DateTime, List <XmlNode>>();

            while (RecordNode != null)
            {
                // depending on frequency, and start date, add the gift to the batch list
                string frequency = TXMLParser.GetAttribute(RecordNode, "frequency");

                int monthStep = 0;

                DateTime startdate = Convert.ToDateTime(TXMLParser.GetAttribute(RecordNode, "startdate"));

                DateTime dateForGift = startdate;

                if (frequency == "once")
                {
                    // no further gift, leave at 0
                }
                else if (frequency == "monthly")
                {
                    monthStep = 1;
                }
                else if (frequency == "quarterly")
                {
                    monthStep = 3;
                }

                do
                {
                    if (!GiftsPerDate.ContainsKey(dateForGift))
                    {
                        GiftsPerDate.Add(dateForGift, new List <XmlNode>());
                    }

                    GiftsPerDate[dateForGift].Add(RecordNode);

                    dateForGift = dateForGift.AddMonths(monthStep);
                } while (monthStep > 0 && dateForGift.Year <= startdate.Year + 3);

                RecordNode = RecordNode.NextSibling;
            }

            return GiftsPerDate;
        }

        private static GiftBatchTDS CreateGiftBatches(SortedList <DateTime, List <XmlNode>>AGiftsPerDate, int APeriodNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();
            ALedgerTable LedgerTable = null;

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // get a list of potential donors (all class FAMILY)
                string sqlGetFamilyPartnerKeys = "SELECT p_partner_key_n FROM PUB_p_family";
                DataTable FamilyKeys = DBAccess.GDBAccessObj.SelectDT(sqlGetFamilyPartnerKeys, "keys", ReadTransaction);

                // get a list of workers (all class FAMILY, with special type WORKER)
                string sqlGetWorkerPartnerKeys =
                    "SELECT PUB_p_family.p_partner_key_n FROM PUB_p_family, PUB_p_partner_type WHERE PUB_p_partner_type.p_partner_key_n = PUB_p_family.p_partner_key_n AND p_type_code_c = 'WORKER'";
                DataTable WorkerKeys = DBAccess.GDBAccessObj.SelectDT(sqlGetWorkerPartnerKeys, "keys", ReadTransaction);

                // get a list of fields (all class UNIT, with unit type F)
                string sqlGetFieldPartnerKeys =
                    String.Format(
                        "SELECT U.p_partner_key_n FROM PUB_p_unit U WHERE u_unit_type_code_c = 'F' AND EXISTS (SELECT * FROM PUB_a_valid_ledger_number V WHERE V.a_ledger_number_i = {0} AND V.p_partner_key_n = U.p_partner_key_n)",
                        FLedgerNumber);
                DataTable FieldKeys = DBAccess.GDBAccessObj.SelectDT(sqlGetFieldPartnerKeys, "keys", ReadTransaction);

                // get a list of key ministries (all class UNIT, with unit type KEY-MIN), and their field ledger number and cost centre code
                string sqlGetKeyMinPartnerKeys =
                    "SELECT u.p_partner_key_n, us.um_parent_unit_key_n, vl.a_cost_centre_code_c " +
                    "FROM PUB_p_unit u, PUB_um_unit_structure us, PUB_a_valid_ledger_number vl " +
                    "WHERE u.u_unit_type_code_c = 'KEY-MIN' " +
                    "AND us.um_child_unit_key_n = u.p_partner_key_n " +
                    "AND vl.p_partner_key_n = us.um_parent_unit_key_n " +
                    "AND vl.a_ledger_number_i = " + FLedgerNumber.ToString();
                DataTable KeyMinistries = DBAccess.GDBAccessObj.SelectDT(sqlGetKeyMinPartnerKeys, "keys", ReadTransaction);

                LedgerTable = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, ReadTransaction);

                AAccountingPeriodRow AccountingPeriodRow = AAccountingPeriodAccess.LoadByPrimaryKey(FLedgerNumber, APeriodNumber, ReadTransaction)[0];

                // create a gift batch for each day.
                // TODO: could create one batch per month, if there are not so many gifts (less than 100 per month)
                foreach (DateTime GlEffectiveDate in AGiftsPerDate.Keys)
                {
                    if ((GlEffectiveDate.CompareTo(AccountingPeriodRow.PeriodStartDate) < 0)
                        || (GlEffectiveDate.CompareTo(AccountingPeriodRow.PeriodEndDate) > 0))
                    {
                        // only create gifts in that period
                        continue;
                    }

                    AGiftBatchRow giftBatch = TGiftBatchFunctions.CreateANewGiftBatchRow(ref MainDS,
                        ref ReadTransaction,
                        ref LedgerTable,
                        FLedgerNumber,
                        GlEffectiveDate);

                    giftBatch.BatchDescription = "Benerator Batch for " + GlEffectiveDate.ToShortDateString();
                    giftBatch.BatchTotal = 0.0m;

                    foreach (XmlNode RecordNode in AGiftsPerDate[GlEffectiveDate])
                    {
                        AGiftRow gift = MainDS.AGift.NewRowTyped();
                        gift.LedgerNumber = giftBatch.LedgerNumber;
                        gift.BatchNumber = giftBatch.BatchNumber;
                        gift.GiftTransactionNumber = giftBatch.LastGiftNumber + 1;
                        gift.DateEntered = GlEffectiveDate;

                        // set donorKey
                        int donorID = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "donor")) % FamilyKeys.Rows.Count;
                        gift.DonorKey = Convert.ToInt64(FamilyKeys.Rows[donorID].ItemArray[0]);

                        // calculate gift detail information
                        int countDetails = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "splitgift"));

                        for (int counter = 1; counter <= countDetails; counter++)
                        {
                            AGiftDetailRow giftDetail = MainDS.AGiftDetail.NewRowTyped();
                            giftDetail.LedgerNumber = gift.LedgerNumber;
                            giftDetail.BatchNumber = gift.BatchNumber;
                            giftDetail.GiftTransactionNumber = gift.GiftTransactionNumber;

                            giftDetail.MotivationGroupCode = "GIFT";
                            giftDetail.GiftTransactionAmount = Convert.ToDecimal(TXMLParser.GetAttribute(RecordNode, "amount_" + counter.ToString()));
                            giftDetail.GiftAmount = giftDetail.GiftTransactionAmount;
                            giftBatch.BatchTotal += giftDetail.GiftAmount;

                            string motivation = TXMLParser.GetAttribute(RecordNode, "motivation_" + counter.ToString());

                            if (motivation == "SUPPORT")
                            {
                                if (WorkerKeys.Rows.Count == 0)
                                {
                                    continue;
                                }

                                giftDetail.MotivationDetailCode = "SUPPORT";
                                int recipientID =
                                    Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "recipient_support_" +
                                            counter.ToString())) % WorkerKeys.Rows.Count;
                                giftDetail.RecipientKey = Convert.ToInt64(WorkerKeys.Rows[recipientID].ItemArray[0]);

                                // TODO: ignore this gift detail, if there is no valid commitment period for the worker
                                // if (InvalidCommitment(giftBatch.GlEffectiveDate)) continue;

                                // TODO giftDetail.RecipientLedgerNumber =
                            }
                            else if (motivation == "FIELD")
                            {
                                if (FieldKeys.Rows.Count == 0)
                                {
                                    continue;
                                }

                                giftDetail.MotivationDetailCode = "FIELD";
                                int recipientID =
                                    Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "recipient_field_" +
                                            counter.ToString())) % FieldKeys.Rows.Count;
                                giftDetail.RecipientKey = Convert.ToInt64(FieldKeys.Rows[recipientID].ItemArray[0]);
                                giftDetail.RecipientLedgerNumber = giftDetail.RecipientKey;
                                giftDetail.CostCentreCode = (giftDetail.RecipientKey / 10000).ToString("0000");
                            }
                            else if (motivation == "KEYMIN")
                            {
                                if (KeyMinistries.Rows.Count == 0)
                                {
                                    continue;
                                }

                                giftDetail.MotivationDetailCode = "KEYMIN";
                                int recipientID =
                                    Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "recipient_keymin_" +
                                            counter.ToString())) % KeyMinistries.Rows.Count;
                                giftDetail.RecipientKey = Convert.ToInt64(KeyMinistries.Rows[recipientID].ItemArray[0]);

                                giftDetail.RecipientLedgerNumber = Convert.ToInt64(KeyMinistries.Rows[recipientID].ItemArray[1]);
                                // TTransactionWebConnector.GetRecipientLedgerNumber(giftDetail.RecipientKey);
                                giftDetail.CostCentreCode = KeyMinistries.Rows[recipientID].ItemArray[2].ToString();
                                // TTransactionWebConnector.IdentifyPartnerCostCentre(FLedgerNumber, giftDetail.RecipientLedgerNumber);
                            }

                            giftDetail.DetailNumber = gift.LastDetailNumber + 1;
                            MainDS.AGiftDetail.Rows.Add(giftDetail);
                            gift.LastDetailNumber = giftDetail.DetailNumber;
                        }

                        if (gift.LastDetailNumber > 0)
                        {
                            MainDS.AGift.Rows.Add(gift);
                            giftBatch.LastGiftNumber = gift.GiftTransactionNumber;
                        }
                    }

                    if (TLogging.DebugLevel > 0)
                    {
                        TLogging.Log(
                            GlEffectiveDate.ToShortDateString() + " " + giftBatch.LastGiftNumber.ToString());
                    }
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            // need to save the last gift batch number in a_ledger
            if (LedgerTable != null)
            {
                TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                TVerificationResultCollection VerificationResult;

                ALedgerAccess.SubmitChanges(LedgerTable, WriteTransaction, out VerificationResult);

                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return MainDS;
        }
    }
}