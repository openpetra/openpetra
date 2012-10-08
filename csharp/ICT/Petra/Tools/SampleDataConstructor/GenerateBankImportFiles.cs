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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Globalization;
using System.Text;
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
    /// tools for generating bank import files
    /// </summary>
    public class SampleDataBankImportFiles
    {
        /// LedgerNumber to be set from outside
        public static int FLedgerNumber = 43;

        /// <summary>
        /// export a number of gift batches to a file
        /// </summary>
        public static void ExportGiftBatches(string AOutputPath)
        {
            // store all gift batches in first period of current year
            string SelectGiftBatchesJanuaryFromAndWhere =
                "FROM PUB_a_ledger, PUB_a_gift_batch, PUB_a_gift, PUB_a_gift_detail, " +
                "   PUB_p_partner donor, PUB_p_partner_banking_details, PUB_p_banking_details_usage, PUB_p_banking_details, PUB_p_bank bank " +
                "WHERE PUB_a_ledger.a_ledger_number_i = " + FLedgerNumber.ToString() + " " +
                "AND PUB_a_gift_batch.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i " +
                "AND PUB_a_gift_batch.a_batch_year_i = PUB_a_ledger.a_current_financial_year_i " +
                "AND PUB_a_gift_batch.a_batch_period_i = 1 " +
                "AND PUB_a_gift.a_ledger_number_i = PUB_a_gift_batch.a_ledger_number_i " +
                "AND PUB_a_gift.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i " +
                "AND PUB_a_gift_detail.a_ledger_number_i = PUB_a_gift.a_ledger_number_i " +
                "AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift.a_batch_number_i " +
                "AND PUB_a_gift_detail.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i " +
                "AND donor.p_partner_key_n = PUB_a_gift.p_donor_key_n " +
                "AND PUB_p_partner_banking_details.p_partner_key_n = PUB_a_gift.p_donor_key_n " +
                "AND PUB_p_banking_details_usage.p_partner_key_n = PUB_a_gift.p_donor_key_n " +
                "AND PUB_p_banking_details_usage.p_banking_details_key_i = PUB_p_partner_banking_details.p_banking_details_key_i " +
                "AND PUB_p_banking_details_usage.p_type_c = 'MAIN' " +
                "AND PUB_p_banking_details.p_banking_details_key_i = PUB_p_partner_banking_details.p_banking_details_key_i " +
                "AND bank.p_partner_key_n = PUB_p_banking_details.p_bank_key_n ";

            BankImportTDS MainDS = new BankImportTDS();
            AGiftBatchTable batches = new AGiftBatchTable();

            string SelectGiftBatchesJanuary =
                "SELECT DISTINCT PUB_a_gift_batch.* " + SelectGiftBatchesJanuaryFromAndWhere;

            DBAccess.GDBAccessObj.SelectDT(batches, SelectGiftBatchesJanuary, null, new OdbcParameter[0], 0, 0);

            string SelectGiftDetailsJanuary =
                "SELECT DISTINCT PUB_a_gift_detail.*, donor.p_partner_key_n AS DonorKey " + SelectGiftBatchesJanuaryFromAndWhere;
            DBAccess.GDBAccessObj.Select(MainDS, SelectGiftDetailsJanuary, MainDS.AGiftDetail.TableName, null);

            // get all banking details of donors involved in this gift batch
            string SelectBankingDetailsJanuary =
                "SELECT DISTINCT PUB_p_banking_details.*, bank.p_branch_code_c AS BankSortCode, donor.p_partner_key_n AS PartnerKey " +
                SelectGiftBatchesJanuaryFromAndWhere;
            DBAccess.GDBAccessObj.Select(MainDS, SelectBankingDetailsJanuary, MainDS.PBankingDetails.TableName, null);

            MainDS.AGiftDetail.DefaultView.Sort = AGiftDetailTable.GetBatchNumberDBName();
            MainDS.PBankingDetails.DefaultView.Sort = BankImportTDSPBankingDetailsTable.GetPartnerKeyDBName();

            int bankStatementCounter = 1000;
            decimal balance = 34304.33m;

            foreach (AGiftBatchRow batch in batches.Rows)
            {
                StoreMT940File(MainDS, AOutputPath, batch.BatchNumber, batch.GlEffectiveDate, bankStatementCounter, ref balance);
                bankStatementCounter++;
            }
        }

        private static void StoreMT940File(BankImportTDS AMainDS, string AOutputPath, int ABatchNumber,
            DateTime ADateEffective,
            int AStatementCounter,
            ref decimal ABalance)
        {
            string outfile = Path.GetFullPath(AOutputPath + Path.DirectorySeparatorChar + ADateEffective.ToString("yyyy-MMM-dd") + ".sta");

            StreamWriter sw = new StreamWriter(outfile, false, Encoding.UTF8);

            sw.WriteLine(":20:STARTUMS");
            sw.WriteLine(":25:20090500/0006853030");
            sw.WriteLine(":28C:" + AStatementCounter.ToString("00000") + "/001");
            sw.WriteLine(":60F:C" + ADateEffective.AddDays(-1).ToString("yyMMdd") + "EUR" +
                ABalance.ToString(CultureInfo.InvariantCulture.NumberFormat).Replace(".", ","));

            DataView GiftView = new DataView(AMainDS.AGiftDetail);
            GiftView.Sort = AGiftDetailTable.GetBatchNumberDBName() + "," +
                            AGiftDetailTable.GetGiftTransactionNumberDBName();

            DataRowView[] giftDetails = AMainDS.AGiftDetail.DefaultView.FindRows(ABatchNumber);

            foreach (DataRowView rv in giftDetails)
            {
                BankImportTDSAGiftDetailRow giftDetail = (BankImportTDSAGiftDetailRow)rv.Row;

                if (giftDetail.DetailNumber == 1)
                {
                    // are there any other gift details for this gift?
                    string AndOthers = string.Empty;
                    decimal Amount = giftDetail.GiftTransactionAmount;

                    DataRowView[] otherGifts = GiftView.FindRows(new object[] { ABatchNumber, giftDetail.GiftTransactionNumber });

                    if (otherGifts.Length > 1)
                    {
                        Amount = 0;
                        AndOthers = " and others";

                        foreach (DataRowView rv2 in otherGifts)
                        {
                            BankImportTDSAGiftDetailRow otherGiftDetail = (BankImportTDSAGiftDetailRow)rv2.Row;
                            Amount += otherGiftDetail.GiftTransactionAmount;
                        }
                    }

                    BankImportTDSPBankingDetailsRow bankingDetails = (BankImportTDSPBankingDetailsRow)
                                                                     AMainDS.PBankingDetails.DefaultView.FindRows(giftDetail.DonorKey)[0].Row;

                    sw.WriteLine(":61:" + ADateEffective.ToString("yyMMdd") + ADateEffective.ToString("MMdd") +
                        "C" + Amount.ToString(CultureInfo.InvariantCulture.NumberFormat).Replace(".", ",") + "N" +
                        "051" + "NONREF");
                    sw.WriteLine(":86:051?00Gutschrift?10999?20" + giftDetail.RecipientKey.ToString() + "?21" + giftDetail.MotivationDetailCode +
                        AndOthers +
                        "?30" + bankingDetails.BankSortCode + "?31" + bankingDetails.BankAccountNumber +
                        "?32" + bankingDetails.AccountName);

                    ABalance += Amount;
                }
            }

            sw.WriteLine(":62F:C" + ADateEffective.ToString("yyMMdd") + "EUR" +
                ABalance.ToString(CultureInfo.InvariantCulture.NumberFormat).Replace(".", ","));

            sw.Close();

            TLogging.Log("mt940 file has been written to: " + outfile);
        }
    }
}